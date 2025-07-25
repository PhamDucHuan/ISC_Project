﻿using Microsoft.EntityFrameworkCore;
using ISC_Project.Data; 
using ISC_Project.Models;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AcceptingSchoolTransfer;
using ISC_Project.Interface;

namespace ISC_Project.Services
{
    public class AcceptingSchoolTransferService : IAcceptingSchoolTransferService
    {
        private readonly ISC_ProjectDbContext _context;

        public AcceptingSchoolTransferService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AcceptingSchoolTransferDto>> GetAllAsync()
        {
            return await _context.AcceptingSchoolTransfers
                                 .Select(ast => new AcceptingSchoolTransferDto
                                 {
                                     AcceptingSchoolTransfersId = ast.AcceptingSchoolTransfersId,
                                     MoveInDate = ast.MoveInDate,
                                     SemesterMoveIn = ast.SemesterMoveIn,
                                     Province = ast.Province,
                                     District = ast.District,
                                     ConvertFrom = ast.ConvertFrom,
                                     Reason = ast.Reason,
                                     FileUrl = ast.FileUrl,
                                     UserId = ast.UserId,
                                     SchoolYearId = ast.SchoolYearId
                                 })
                                 .ToListAsync();
        }

        public async Task<AcceptingSchoolTransferDto?> GetByIdAsync(int id)
        {
            var ast = await _context.AcceptingSchoolTransfers.FindAsync(id);
            if (ast == null)
            {
                return null;
            }

            return new AcceptingSchoolTransferDto
            {
                AcceptingSchoolTransfersId = ast.AcceptingSchoolTransfersId,
                MoveInDate = ast.MoveInDate,
                SemesterMoveIn = ast.SemesterMoveIn,
                Province = ast.Province,
                District = ast.District,
                ConvertFrom = ast.ConvertFrom,
                Reason = ast.Reason,
                FileUrl = ast.FileUrl,
                UserId = ast.UserId,
                SchoolYearId = ast.SchoolYearId
            };
        }

        public async Task<AcceptingSchoolTransferDto> CreateAsync(CreateAcceptingSchoolTransferDto createDto)
        {
            var ast = new AcceptingSchoolTransfer
            {
                MoveInDate = createDto.MoveInDate,
                SemesterMoveIn = createDto.SemesterMoveIn,
                Province = createDto.Province,
                District = createDto.District,
                ConvertFrom = createDto.ConvertFrom,
                Reason = createDto.Reason,
                FileUrl = createDto.FileUrl,
                UserId = createDto.UserId,
                SchoolYearId = createDto.SchoolYearId
            };

            _context.AcceptingSchoolTransfers.Add(ast);
            await _context.SaveChangesAsync();

            return new AcceptingSchoolTransferDto
            {
                AcceptingSchoolTransfersId = ast.AcceptingSchoolTransfersId,
                MoveInDate = ast.MoveInDate,
                SemesterMoveIn = ast.SemesterMoveIn,
                Province = ast.Province,
                District = ast.District,
                ConvertFrom = ast.ConvertFrom,
                Reason = ast.Reason,
                FileUrl = ast.FileUrl,
                UserId = ast.UserId,
                SchoolYearId = ast.SchoolYearId
            };
        }
    }
}