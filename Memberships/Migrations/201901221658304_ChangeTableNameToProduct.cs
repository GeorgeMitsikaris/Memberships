namespace Memberships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTableNameToProduct : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Products", newName: "Product");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Product", newName: "Products");
        }
    }
}
