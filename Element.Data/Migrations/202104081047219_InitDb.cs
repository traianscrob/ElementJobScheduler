namespace Element.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Element.JobHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        JobId = c.Guid(nullable: false),
                        Description = c.String(),
                        RunSuccessfuly = c.Boolean(nullable: false),
                        ExecutionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Element.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "Element.Jobs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CronExpression = c.String(),
                        LastSuccessfulRun = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Element.JobHistories", "JobId", "Element.Jobs");
            DropIndex("Element.JobHistories", new[] { "JobId" });
            DropTable("Element.Jobs");
            DropTable("Element.JobHistories");
        }
    }
}
