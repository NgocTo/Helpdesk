// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents the Department entity in the Helpdesk Data Access Layer (DAL).
// ============================================================================

namespace HelpdeskDAL;

public partial class Department : HelpdeskEntity
{
    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
