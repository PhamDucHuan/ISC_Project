﻿using ISC_Project.DTOs;
using ISC_Project.DTOs.Assignment;

namespace ISC_Project.Services
{
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDto>> GetAllAsync();
        Task<AssignmentDto?> GetByIdAsync(int id);
        Task<AssignmentDto> CreateAsync(CreateAssignmentDto createDto);
    }
}