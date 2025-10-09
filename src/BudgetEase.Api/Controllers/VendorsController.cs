using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorsController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IEventRepository _eventRepository;

    public VendorsController(IVendorRepository vendorRepository, IEventRepository eventRepository)
    {
        _vendorRepository = vendorRepository;
        _eventRepository = eventRepository;
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendorsByEvent(int eventId)
    {
        var vendors = await _vendorRepository.GetAllByEventIdAsync(eventId);
        var vendorDtos = vendors.Select(v => new VendorDto
        {
            Id = v.Id,
            EventId = v.EventId,
            Name = v.Name,
            ServiceType = v.ServiceType,
            ContactNumber = v.ContactNumber,
            Email = v.Email,
            PaymentTerms = v.PaymentTerms,
            NextReminderDate = v.NextReminderDate
        });

        return Ok(vendorDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VendorDto>> GetVendor(int id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null)
            return NotFound();

        var vendorDto = new VendorDto
        {
            Id = vendor.Id,
            EventId = vendor.EventId,
            Name = vendor.Name,
            ServiceType = vendor.ServiceType,
            ContactNumber = vendor.ContactNumber,
            Email = vendor.Email,
            PaymentTerms = vendor.PaymentTerms,
            NextReminderDate = vendor.NextReminderDate
        };

        return Ok(vendorDto);
    }

    [HttpPost]
    public async Task<ActionResult<VendorDto>> CreateVendor(CreateVendorDto dto)
    {
        var eventExists = await _eventRepository.ExistsAsync(dto.EventId);
        if (!eventExists)
            return BadRequest("Event not found");

        var vendor = new Vendor
        {
            EventId = dto.EventId,
            Name = dto.Name,
            ServiceType = dto.ServiceType,
            ContactNumber = dto.ContactNumber,
            Email = dto.Email,
            PaymentTerms = dto.PaymentTerms,
            NextReminderDate = dto.NextReminderDate
        };

        await _vendorRepository.CreateAsync(vendor);

        var vendorDto = new VendorDto
        {
            Id = vendor.Id,
            EventId = vendor.EventId,
            Name = vendor.Name,
            ServiceType = vendor.ServiceType,
            ContactNumber = vendor.ContactNumber,
            Email = vendor.Email,
            PaymentTerms = vendor.PaymentTerms,
            NextReminderDate = vendor.NextReminderDate
        };

        return CreatedAtAction(nameof(GetVendor), new { id = vendor.Id }, vendorDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VendorDto>> UpdateVendor(int id, UpdateVendorDto dto)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null)
            return NotFound();

        if (dto.Name != null) vendor.Name = dto.Name;
        if (dto.ServiceType != null) vendor.ServiceType = dto.ServiceType;
        if (dto.ContactNumber != null) vendor.ContactNumber = dto.ContactNumber;
        if (dto.Email != null) vendor.Email = dto.Email;
        if (dto.PaymentTerms != null) vendor.PaymentTerms = dto.PaymentTerms;
        if (dto.NextReminderDate.HasValue) vendor.NextReminderDate = dto.NextReminderDate;

        await _vendorRepository.UpdateAsync(vendor);

        var vendorDto = new VendorDto
        {
            Id = vendor.Id,
            EventId = vendor.EventId,
            Name = vendor.Name,
            ServiceType = vendor.ServiceType,
            ContactNumber = vendor.ContactNumber,
            Email = vendor.Email,
            PaymentTerms = vendor.PaymentTerms,
            NextReminderDate = vendor.NextReminderDate
        };

        return Ok(vendorDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
        var result = await _vendorRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("reminders")]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetUpcomingReminders()
    {
        var vendors = await _vendorRepository.GetVendorsWithUpcomingRemindersAsync();
        var vendorDtos = vendors.Select(v => new VendorDto
        {
            Id = v.Id,
            EventId = v.EventId,
            Name = v.Name,
            ServiceType = v.ServiceType,
            ContactNumber = v.ContactNumber,
            Email = v.Email,
            PaymentTerms = v.PaymentTerms,
            NextReminderDate = v.NextReminderDate
        });

        return Ok(vendorDtos);
    }
}
