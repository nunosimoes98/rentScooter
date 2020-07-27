namespace eRecarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                Insert into AspNetUsers (Id, UserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, PasswordHash, SecurityStamp) VALUES
                   ('0C4C7F27-B56B-4435-B2C6-F6C57E3ACF54', 'admin@erecarga.pt', 'admin@erecarga.pt', 1, 0, 0, 1, 0,
                    'ALZFjKdobNdiEmlpH0pIlCXek+sXx0E/6aBhubLYrNWDPNEQGm1mDbAiQ0DwXhdoTA==', '956fd74d-2e2b-44d9-8e44-8ef1023aea37');
                Insert into AspNetRoles (Id, Name) VALUES ('0c3f854c-6ebd-46a6-b185-ee48f74a4aa4', 'Admin');
                Insert into AspNetUserRoles (UserId, RoleId) VALUES ('0C4C7F27-B56B-4435-B2C6-F6C57E3ACF54', '0c3f854c-6ebd-46a6-b185-ee48f74a4aa4');
            ");

            // Password de Admin = Admin1!
    }
        
        public override void Down()
        {
            Sql(@"
                DELETE from AspNetUserRoles Where UserId = '0C4C7F27-B56B-4435-B2C6-F6C57E3ACF54';
                DELETE from AspNetRoles Where Id = '0c3f854c-6ebd-46a6-b185-ee48f74a4aa4';
                DELETE from AspNetUsers Where Id = '0C4C7F27-B56B-4435-B2C6-F6C57E3ACF54';
            ");
        }
    }
}
