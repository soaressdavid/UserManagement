namespace UserManagement.API.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;

        public bool IsDeleted { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private User() { }

        public User(string name, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = "User";
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetName(string name) => Name = name;

        public void SetEmail(string email) => Email = email;
        public void SetPassword(string password) => PasswordHash = password;
        public void SoftDelete() => IsDeleted = true;
    }
}
