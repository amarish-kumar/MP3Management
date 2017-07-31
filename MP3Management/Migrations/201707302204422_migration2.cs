namespace MP3Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MP3File", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.MP3File", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.MP3File", "AlbumName", c => c.String(nullable: false));
            AlterColumn("dbo.Playlist", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Playlist", "Name", c => c.String());
            AlterColumn("dbo.MP3File", "AlbumName", c => c.String());
            AlterColumn("dbo.MP3File", "Author", c => c.String());
            AlterColumn("dbo.MP3File", "Name", c => c.String());
        }
    }
}
