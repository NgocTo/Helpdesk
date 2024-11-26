// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents the Problem entity in the Helpdesk Data Access Layer (DAL).
// ============================================================================

namespace HelpdeskDAL;

public partial class Problem : HelpdeskEntity
{
    public string? Description { get; set; }

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
}
