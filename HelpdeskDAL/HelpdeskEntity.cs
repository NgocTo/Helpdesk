// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This base class provides a common structure for entities in the application,
// containing a primary key, Id, and a concurrency control property Timer.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? Timer { get; set; }
    }
}