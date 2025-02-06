using AutoMapper;
using Moq;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Services;
using PensionManagement.Domain.Entities;

namespace PensionManagement.Tests;

public class MemberServiceTests
{
    private readonly Mock<IMemberRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MemberService _memberService;

    public MemberServiceTests()
    {
        _repositoryMock = new Mock<IMemberRepository>();
        _mapperMock = new Mock<IMapper>();
        _memberService = new MemberService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedMemberDtos()
    {
        // Arrange
        var members = new List<Member>
        {
            new Member { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new Member { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };
        var memberDtos = new List<MemberDto>
        {
            new MemberDto { Id = members[0].Id, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new MemberDto { Id = members[1].Id, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(members);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<MemberDto>>(members)).Returns(memberDtos);

        // Act
        var result = await _memberService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, memberDtos.Count);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<MemberDto>>(members), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedMemberDto_WhenMemberExists()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var member = new Member { Id = memberId, FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var memberDto = new MemberDto { Id = memberId, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(memberId)).ReturnsAsync(member);
        _mapperMock.Setup(mapper => mapper.Map<MemberDto>(member)).Returns(memberDto);

        // Act
        var result = await _memberService.GetByIdAsync(memberId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(memberDto.Id, result.Id);
        Assert.Equal(memberDto.FirstName, result.FirstName);
        Assert.Equal(memberDto.Email, result.Email);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(memberId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<MemberDto>(member), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var memberDto = new MemberDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var member = new Member { Id = memberDto.Id, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

        _mapperMock.Setup(mapper => mapper.Map<Member>(memberDto)).Returns(member);
        _repositoryMock.Setup(repo => repo.AddAsync(member)).Returns(Task.CompletedTask);

        // Act
        await _memberService.AddAsync(memberDto);

        // Assert
        _repositoryMock.Verify(repo => repo.AddAsync(member), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<Member>(memberDto), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingMember_WhenMemberExists()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var existingMember = new Member { Id = memberId, FirstName = "Old Name", Email = "old@example.com" };
        var updatedMemberDto = new MemberDto { Id = memberId, FirstName = "New Name", Email = "new@example.com" };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(memberId)).ReturnsAsync(existingMember);
        _repositoryMock.Setup(repo => repo.UpdateAsync(existingMember)).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map(updatedMemberDto, existingMember));

        // Act
        await _memberService.UpdateAsync(memberId, updatedMemberDto);

        // Assert
        Assert.Equal("New Name", existingMember.FirstName);
        Assert.Equal("new@example.com", existingMember.Email);
        _repositoryMock.Verify(repo => repo.UpdateAsync(existingMember), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(updatedMemberDto, existingMember), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        _repositoryMock.Setup(repo => repo.DeleteAsync(memberId)).Returns(Task.CompletedTask);

        // Act
        await _memberService.DeleteAsync(memberId);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteAsync(memberId), Times.Once);
    }
}