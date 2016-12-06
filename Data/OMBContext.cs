//Se utiliza para entrar al Entity Framework
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Data
{
  /// <summary>
  /// SINGLETON
  /// Retorna en la propiedad DB un contexto para acceder a las diferentes colecciones
  /// </summary>
  public class OMBContext : DbContext
  {
    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Localidad> Localidades { get; set; }

    public DbSet<Provincia> Provincias { get; set; }

    public DbSet<TipoContacto> TiposContacto { get; set; }

    public DbSet<TipoDocumento> TiposDeDocumento { get; set; }

    public DbSet<Persona> Personas { get; set; }

    //public DbSet<CategoriaEmpleado> CategoriasEmpleado { get; set; }

    public DbSet<Empleado> Empleados { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    //  Agregamos libros y editoriales

    public DbSet<Libro> Libros { get; set; }

    public DbSet<Editorial> Editoriales { get; set; }

    private StreamWriter writer;

    public static OMBContext DB
    {
      get
      {
        if (_ctx == null)
          _ctx = new OMBContext();

        return _ctx;
      }
    }

    private static OMBContext _ctx;

    private OMBContext()
    {
      //Configuration.LazyLoadingEnabled = false;

#if CS5
      writer = File.CreateText(string.Format(@"{0}\{1}.LOG", Environment.CurrentDirectory, this.GetType().Name));
#else
      writer = File.CreateText($@"{Environment.CurrentDirectory}\{this.GetType().Name}.LOG");
#endif
      //  writer = File.CreateText(string.Format(@"{0}\{1}.LOG", "F:\\", this.GetType().Name));
      Database.Log = writer.WriteLine;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      writer.Close();
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new ConfigurarCategoria());
      modelBuilder.Configurations.Add(new ConfigurarTiposDocumento());

      modelBuilder.Configurations.Add(new ConfigurarProvincia());
      modelBuilder.Configurations.Add(new ConfigurarLocalidad());

      modelBuilder.Configurations.Add(new ConfigurarPersona());
      modelBuilder.Configurations.Add(new ConfigurarContacto());
      modelBuilder.Configurations.Add(new ConfigurarTipoContacto());

      modelBuilder.Configurations.Add(new ConfigurarEmpleado());
      modelBuilder.Configurations.Add(new ConfigurarUsuario());

      modelBuilder.Configurations.Add(new ConfigurarEditorial());
    }

    //protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
    //{
    //  if (entityEntry.Entity is Libro)
    //    return false;

    //  return base.ShouldValidateEntity(entityEntry);
    //}

    public void MostrarCambios([CallerMemberName] string header = null)
    {
      if (header != null)
      {
        Console.WriteLine(new string('=', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('=', header.Length));
      }

      //  DbEntityEntry
      foreach (var entry in ChangeTracker.Entries())
      {
        Console.WriteLine("Tipo de la entidad: {0} ; State: {1}", entry.Entity.GetType().FullName, entry.State);
      }
    }
  }

  /*
   * Si quiero configurar aspectos de EF que no estan en App.config (ojo que los del archivo siempre tienen mas prioridad!!)
   * por ejemplo si voy a crear conexiones por defecto a LocalDB o a SQL o a SQL Compact...
   * La clase tiene que estar ACA para que EF detecte que desciende de DbConfiguration y la utilice!!


    public class OMBConfiguration : DbConfiguration
    {
      public OMBConfiguration()
      {
        this.SetDefaultConnectionFactory(new LocalDbConnectionFactory("ProjectsV13"));
        this.SetDefaultConnectionFactory(new SqlConnectionFactory());
      }
    }
  */
  public class ConfigurarCategoria : EntityTypeConfiguration<Categoria>
  {
    public ConfigurarCategoria()
    {
      this.HasKey(categ => categ.IDCategoria);
      this.Property(cat => cat.IDCategoria)
        .HasColumnName("ID_Categoria");

      //  para SQL Compact, eliminar la opcion siguiente
      //  Property(x => x.CategoriaID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

      this.HasOptional(cat => cat.Parent)
        .WithMany(cat => cat.SubCategorias)
        .Map(conf => conf.MapKey("Categoria_Up"));
    }
  }

  public class ConfigurarLocalidad : EntityTypeConfiguration<Localidad>
  {
    public ConfigurarLocalidad()
    {
      this.ToTable("Localidades");
      this.HasKey(loc => loc.IDLocalidad);

      this.Property(loc => loc.IDLocalidad)
        .HasColumnName("ID_Localidad");
      this.Property(loc => loc.Nombre)
        .HasColumnName("Localidad");

      this.HasRequired(loc => loc.Provincia)
        .WithMany(prov => prov.Localidades)
        .Map(cfg => cfg.MapKey("ID_Provincia"));
    }
  }

  public class ConfigurarProvincia : EntityTypeConfiguration<Provincia>
  {
    public ConfigurarProvincia()
    {
      this.HasKey(prov => prov.IDProvincia);

      this.Property(prov => prov.Nombre)
        .HasColumnName("Provincia");

      this.Property(prov => prov.IDProvincia)
        .HasColumnName("ID_Provincia")
        .IsFixedLength()
        .HasMaxLength(1);
    }
  }

  public class ConfigurarTiposDocumento : EntityTypeConfiguration<TipoDocumento>
  {
    public ConfigurarTiposDocumento()
    {
      this.ToTable("TiposDeDocumento");
      this.HasKey(tipo => tipo.IDTipoDocumento);
      this.Property(tipo => tipo.IDTipoDocumento)
        .HasColumnName("ID_TipoDocumento");
    }
  }

  public class ConfigurarPersona : EntityTypeConfiguration<Persona>
  {
    public ConfigurarPersona()
    {
      this.HasKey(per => per.IDPersona);
      this.Property(per => per.IDPersona)
        .HasColumnName("ID_Persona");

      this.HasMany(per => per.InfoContacto)
        .WithRequired()
        .Map(cfg => cfg.MapKey("ID_Persona"));

      this.HasOptional(per => per.Localidad)
        .WithMany()
        .Map(cfg => cfg.MapKey("ID_Localidad"));

      this.HasOptional(per => per.TipoDocumento)
        .WithMany()
        .Map(cfg => cfg.MapKey("ID_TipoDocumento"));
    }
  }

  public class ConfigurarContacto : EntityTypeConfiguration<Contacto>
  {
    public ConfigurarContacto()
    {
      this.HasKey(cont => cont.IDContacto);

      this.ToTable("Contactos");

      this.Property(cont => cont.IDContacto)
        .HasColumnName("ID_Contacto");

      this.HasRequired(cont => cont.Tipo)
        .WithMany()
        .Map(cfg => cfg.MapKey("ID_TipoContacto"));
    }
  }

  public class ConfigurarTipoContacto : EntityTypeConfiguration<TipoContacto>
  {
    public ConfigurarTipoContacto()
    {
      this.ToTable("TiposDeContacto");

      this.HasKey(tcon => tcon.IDTipoContacto);

      this.Property(tcon => tcon.RegExp)
        .HasColumnName("Validacion");

      this.Property(tcon => tcon.IDTipoContacto)
        .HasColumnName("ID_TipoContacto");
    }
  }

  public class ConfigurarEmpleado : EntityTypeConfiguration<Empleado>
  {
    public ConfigurarEmpleado()
    {
      this.ToTable("Empleados");

      this.HasKey(emp => emp.Legajo);

      this.HasRequired(emp => emp.Persona)
        .WithOptional()
        .Map(cfg => cfg.MapKey("ID_Persona"));
    }
  }

  public class ConfigurarUsuario : EntityTypeConfiguration<Usuario>
  {
    public ConfigurarUsuario()
    {
      this.ToTable("Usuarios");

      this.HasKey(usr => usr.Login);

      this.Property(usr => usr.PasswordExpiration)
        .HasColumnName("FechaExpiracionPassword");

      this.Property(usr => usr.LastFailLogin)
        .HasColumnName("FechaLastLoginBAD");

      this.Property(usr => usr.LastSuccessLogin)
        .HasColumnName("FechaLastLoginOK");

      this.HasRequired(usr => usr.Empleado)
        //  .WithOptional()
        .WithMany()
        .Map(cfg => cfg.MapKey("Legajo"));
    }
  }

  public class ConfigurarEditorial : EntityTypeConfiguration<Editorial>
  {
    public ConfigurarEditorial()
    {
      this.ToTable("Editoriales");

      this.HasKey(edit => edit.IDEditorial);

      this.Property(edit => edit.IDEditorial)
        .HasColumnName("ID_Editorial");

      this.HasOptional(edit => edit.Localidad)
        .WithMany()
        .Map(cfg => cfg.MapKey("ID_Localidad"));
    }
  }
}
