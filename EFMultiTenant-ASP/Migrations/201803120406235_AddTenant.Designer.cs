// <auto-generated />
namespace EFMultiTenant.Models
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class AddTenant : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddTenant));
        
        string IMigrationMetadata.Id
        {
            get { return "201803120406235_AddTenant"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}