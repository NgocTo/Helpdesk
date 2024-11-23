﻿// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents the Employee entity in the Helpdesk Data Access Layer (DAL).
// ============================================================================

namespace HelpdeskDAL;

public partial class Employee : HelpdeskEntity
{
    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public int DepartmentId { get; set; }

    public bool? IsTech { get; set; }

    public byte[]? StaffPicture { get; set; }

    public virtual Department Department { get; set; } = null!;
}