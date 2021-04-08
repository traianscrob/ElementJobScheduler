namespace Element.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Element.JobHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        JobId = c.Guid(nullable: false),
                        Description = c.String(),
                        WasSuccessful = c.Boolean(nullable: false),
                        ExecutionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Element.Jobs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        LastSuccessfulRun = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Element.JobHistories", t => t.Id)
                .Index(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("Element.Jobs", "Id", "Element.JobHistories");
            DropIndex("Element.Jobs", new[] { "Id" });
            DropTable("Element.Jobs");
            DropTable("Element.JobHistories");
        }
    }
}
