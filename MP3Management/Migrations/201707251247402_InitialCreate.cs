namespace MP3Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MP3File",
                c => new
                    {
                        MP3FileID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Author = c.String(),
                        AlbumName = c.String(),
                    })
                .PrimaryKey(t => t.MP3FileID);
            
            CreateTable(
                "dbo.Playlist",
                c => new
                    {
                        PlaylistID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PlaylistID);
            
            CreateTable(
                "dbo.PlaylistMP3File",
                c => new
                    {
                        Playlist_PlaylistID = c.Int(nullable: false),
                        MP3File_MP3FileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Playlist_PlaylistID, t.MP3File_MP3FileID })
                .ForeignKey("dbo.Playlist", t => t.Playlist_PlaylistID, cascadeDelete: true)
                .ForeignKey("dbo.MP3File", t => t.MP3File_MP3FileID, cascadeDelete: true)
                .Index(t => t.Playlist_PlaylistID)
                .Index(t => t.MP3File_MP3FileID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlaylistMP3File", "MP3File_MP3FileID", "dbo.MP3File");
            DropForeignKey("dbo.PlaylistMP3File", "Playlist_PlaylistID", "dbo.Playlist");
            DropIndex("dbo.PlaylistMP3File", new[] { "MP3File_MP3FileID" });
            DropIndex("dbo.PlaylistMP3File", new[] { "Playlist_PlaylistID" });
            DropTable("dbo.PlaylistMP3File");
            DropTable("dbo.Playlist");
            DropTable("dbo.MP3File");
        }
    }
}
