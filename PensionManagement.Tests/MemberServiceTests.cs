using AutoMapper;
using Moq;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Services;
using PensionManagement.Domain.Entities;
using PensionManagement.Domain.Enums;
using FluentAssertions;

namespace PensionManagement.Tests;

public class MemberServiceTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<ITransactionHistoryService> _transactionHistoryServiceMock;
    private readonly IMapper _mapper;
    private readonly MemberService _memberService;

    public MemberServiceTests()
    {
        _memberRepositoryMock = new Mock<IMemberRepository>();
        _transactionHistoryServiceMock = new Mock<ITransactionHistoryService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Member, MemberDto>().ReverseMap();
        });

        _mapper = config.CreateMapper();

        _memberService = new MemberService(
            _memberRepositoryMock.Object,
            _mapper,
            _transactionHistoryServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfMembers()
    {
        // Arrange
        var members = new List<Member>
        {
            new Member { Id = 1, FirstName = "John", LastName = "Doe" },
            new Member { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };

        _memberRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(members);

        // Act
        var result = await _memberService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _memberRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMember_WhenMemberExists()
    {
        // Arrange
        var member = new Member { Id = 1, FirstName = "John", LastName = "Doe" };

        _memberRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(member);

        // Act
        var result = await _memberService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        _memberRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMemberAndLogTransaction()
    {
        // Arrange
        var memberDto = new MemberDto { Id = 1, FirstName = "John", LastName = "Doe" };
        var member = _mapper.Map<Member>(memberDto);

        _memberRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);
        _transactionHistoryServiceMock.Setup(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()));

        // Act
        await _memberService.AddAsync(memberDto);

        // Assert
        _memberRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Member>()), Times.Once);
        _transactionHistoryServiceMock.Verify(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenMemberDoesNotExist()
    {
        // Arrange
        _memberRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Member)null);

        // Act
        Func<Task> act = async () => await _memberService.UpdateAsync(1, new MemberDto());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMemberAndLogTransaction()
    {
        // Arrange
        var existingMember = new Member { Id = 1, FirstName = "John", LastName = "Doe" };
        var updatedMemberDto = new MemberDto { Id = 1, FirstName = "Johnathan", LastName = "Doe" };

        _memberRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingMember);
        _memberRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);
        _transactionHistoryServiceMock.Setup(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()));

        // Act
        await _memberService.UpdateAsync(1, updatedMemberDto);

        // Assert
        existingMember.FirstName.Should().Be("Johnathan");
        _memberRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Member>()), Times.Once);
        _transactionHistoryServiceMock.Verify(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteMemberAndLogTransaction()
    {
        // Arrange
        _memberRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
        _transactionHistoryServiceMock.Setup(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()));

        // Act
        await _memberService.DeleteAsync(1);

        // Assert
        _memberRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        _transactionHistoryServiceMock.Verify(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()), Times.Once);
    }

    [Fact]
    public async Task PartialUpdate_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var existingMember = new Member { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var updateDto = new MemberDto { Id = 1, FirstName = "Johnny" }; // Only updating FirstName

        _memberRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingMember);
        _memberRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);

        // Act
        await _memberService.UpdateAsync(1, updateDto);

        // Assert
        existingMember.FirstName.Should().Be("Johnny");
        existingMember.Email.Should().Be("john@example.com"); // Ensuring Email remains unchanged
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkMemberAsDeleted()
    {
        // Arrange
        var existingMember = new Member { Id = 1, IsDeleted = false };
        _memberRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingMember);
        _memberRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);

        // Act
        await _memberService.DeleteAsync(1);

        // Assert
        existingMember.IsDeleted.Should().BeTrue();
        _memberRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Member>()), Times.Once);
    }

    [Fact]
    public async Task TransactionLog_ShouldContainCorrectDetails()
    {
        // Arrange
        var memberDto = new MemberDto { Id = 1, FirstName = "John", LastName = "Doe" };

        _memberRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);
        _transactionHistoryServiceMock.Setup(svc => svc.LogChange(It.IsAny<TransactionHistoryDto>()));

        // Act
        await _memberService.AddAsync(memberDto);

        // Assert
        _transactionHistoryServiceMock.Verify(svc => svc.LogChange(It.Is<TransactionHistoryDto>(
            t => t.EntityType == EntityType.Member &&
                 t.EntityId == memberDto.Id &&
                 t.ActionType == ActionType.INSERT
        )), Times.Once);
    }
}