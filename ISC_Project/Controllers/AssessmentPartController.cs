﻿using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentPartDto;

using ISC_Project.Models; 
using ISC_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentPartController : ControllerBase
    {
        private readonly IAssessmentPartService _service;

        public AssessmentPartController(IAssessmentPartService service) 
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateAssessmentPartDto createDto) // THAY ĐỔI Ở ĐÂY
        {
            // dd ModelState.IsValid check to catch validation errors from Data Annotations in DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateAsync(createDto); //Call service with DTO
            return CreatedAtAction(nameof(GetById), new { id = created.AssessmentPartsId }, created);
        }
    }
}