﻿namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; } = "Guest"; // e.g., "Cashier", "Manager"
    }
}
