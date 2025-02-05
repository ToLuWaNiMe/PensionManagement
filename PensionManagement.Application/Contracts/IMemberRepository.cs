﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Contracts;

public interface IMemberRepository
{
    Task<Member> GetByIdAsync(Guid id);
    Task<IEnumerable<Member>> GetAllAsync();
    Task AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeleteAsync(Guid id);
}
