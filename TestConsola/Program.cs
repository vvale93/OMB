#define PASO_13

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Entidades;
using Data;
using Servicios;

namespace TestDatabase
{
  class Program
  {
    static void Main(string[] args)
    {
      /*
       * En este codigo no nos interesa separar capas, solo testear el funcionamiento de la base de datos y de los diferentes servicios
       * Podemos usar metodos directamente de DbContext o podemos usar las clases que encapsulan EF
       */
      //  https://www.connectionstrings.com/sql-server-compact/
      //
      //  OMBContext ctx = new OMBContext(@"Data source=F:\CURSO_2016_01\master\OMB\db\OMB.sdf;Persist Security Info=False;");
      OMBContext ctx = OMBContext.DB;

      AppDomain.CurrentDomain.UnhandledException += (o, e) => { ctx.Dispose(); Console.WriteLine("Excepcion"); };

      if (ctx.Database.Exists())
        Console.WriteLine("La base esta!");

#if PASO_1
      //  PASO 1 - Crear root categoty (Libros)
      //
      Categoria root = new Categoria() {Nombre = "Libros"};

      ctx.Categorias.Add(root);
      ctx.SaveChanges();
#endif



#if PASO_2
      //  PASO 2 - Crear categoria hijas (Informatica) [primero obtengo la root y luego creo las child]
      //
      Categoria root = (from cat in ctx.Categorias where cat.Nombre == "Libros" select cat).FirstOrDefault();

      Categoria newCat = new Categoria() { Nombre = "Informatica", Parent = root };

      ctx.Categorias.Add(newCat);
      root?.SubCategorias.Add(newCat);

      newCat = new Categoria() { Nombre = "Ficcion", Parent = root };
      ctx.Categorias.Add(newCat);
      root?.SubCategorias.Add(newCat);


      newCat = new Categoria() { Nombre = "No Ficcion", Parent = root };

      ctx.Categorias.Add(newCat);
      root?.SubCategorias.Add(newCat);

      newCat = new Categoria() { Nombre = "Autoayuda", Parent = root };

      ctx.Categorias.Add(newCat);
      root?.SubCategorias.Add(newCat);

      newCat = new Categoria() { Nombre = "Viajes", Parent = root };

      ctx.Categorias.Add(newCat);
      root?.SubCategorias.Add(newCat);

      ctx.SaveChanges();
#endif

#if PASO_2_5
      //  PASO 2 y MEDIO - Crear categorias hijas de una subcategoria...
      //
      Categoria @base = (from cat in ctx.Categorias where cat.Nombre == "Informatica" select cat).FirstOrDefault();

      //  Categoria newCat = new Categoria() { Nombre = "Informatica", Parent = root };
      //  Categoria newCat = new Categoria() { Nombre = "Ficcion", Parent = root };
      Categoria newCat = new Categoria() { Nombre = "Programacion", Parent = @base };

      ctx.Categorias.Add(newCat);
      @base?.SubCategorias.Add(newCat);

      newCat = new Categoria() { Nombre = "Sistemas Operativos", Parent = @base };

      ctx.Categorias.Add(newCat);
      @base?.SubCategorias.Add(newCat);

      newCat = new Categoria() { Nombre = "Networking", Parent = @base };

      ctx.Categorias.Add(newCat);
      @base?.SubCategorias.Add(newCat);

      ctx.SaveChanges();
#endif


#if PASO_3
      //  PASO 3 - Chequear relaciones!! FULL

      //  Categoria root = (from cat in ctx.Categorias.Include("SubCategorias") where cat.Nombre == "Libros" select cat).FirstOrDefault();
      //  1
      //Categoria categoria = (from cat in ctx.Categorias where cat.Nombre == "Libros" select cat).FirstOrDefault();
      Categoria categoria = (from cat in ctx.Categorias where cat.Nombre == "Informatica" select cat).FirstOrDefault();

      //  2
      Console.WriteLine($"{categoria.IDCategoria} {categoria.Nombre} {categoria.SubCategorias.Count}");

      Console.WriteLine("SUB-CATEGORIAS");

      foreach (Categoria cat in categoria.SubCategorias)
        //  3
        Console.WriteLine($"{cat.IDCategoria} {cat.Nombre} {cat.SubCategorias.Count}");

      Console.WriteLine("PARENT CATEGORIA");
      if (categoria.Parent == null)
        //  4
        Console.WriteLine($"{categoria.Nombre} es ROOT");
      else
        Console.WriteLine($"La categoria parent de {categoria.Nombre} es {categoria.Parent.Nombre}");

#endif

#if PASO_4
      //  PASO 4 - Ingreso de los Tipos de Documentos
      //
      TipoDocumento tipo = new TipoDocumento() {Descripcion = "DNI"};

      ctx.TiposDeDocumento.Add(tipo);

      tipo = new TipoDocumento() { Descripcion = "Pasaporte", Observaciones = ""};
      ctx.TiposDeDocumento.Add(tipo);

      tipo = new TipoDocumento() { Descripcion = "LE", Observaciones = "Libreta de Enrolamiento - Solo para hombre nacidos antes del 1960" };
      ctx.TiposDeDocumento.Add(tipo);

      tipo = new TipoDocumento() { Descripcion = "LC", Observaciones = "Libreta Civica - Solo para mujeres nacidas antes de 1955" };
      ctx.TiposDeDocumento.Add(tipo);

      tipo = new TipoDocumento() { Descripcion = "Documento Pendiente de Ingreso", Observaciones = "Averiguar cuanto antes el tipo y numero de documento!!" };
      ctx.TiposDeDocumento.Add(tipo);

      ctx.SaveChanges();
#endif

#if PASO_4_5
      //  PASO 4_5 - testeamos algunas provincias y localidades...
      var localidades = ctx.Localidades.Where(loc => loc.Nombre.Contains("martin"));

      Console.WriteLine("TODAS LAS LOCALIDADES QUE CONTIENEN <<MARTIN>> EN SU NOMBRE\n\n");

      foreach (Localidad city in localidades)
        Console.WriteLine($"Nombre Localidad: {city.Nombre} ; Provincia: {city.Provincia.Nombre}");

      Console.WriteLine($"Hay {localidades.Count()} localidades cuyo nombre contiene <<MARTIN>>");

      Console.WriteLine("\nPress any key para listar ciudades de FORMOSA");
      Console.ReadLine();

      //  observar que hasta aca no necesito tener Provincias como DbSet...

      Provincia formosa = ctx.Provincias.Where(prov => prov.Nombre == "formosa").FirstOrDefault();

      foreach (var city in formosa.Localidades)
        Console.WriteLine($"Nombre Localidad: {city.Nombre} ; Provincia: {city.Provincia.Nombre}");

      Console.WriteLine($"Hay {formosa.Localidades.Count} localidades en esta provincia");

#endif

#if PASO_4_5_1
      //
      //  Crear nueva Provincia y nueva Localidad
      //  esto normalmente no se haria, pero vemos si funciona (despues borrar desde la DB)

      Localidad nuevaLocalidad = new Localidad() { Nombre = "Plumas Verdes" };
      Provincia nuevaProvincia = new Provincia() { IDProvincia = "1", Nombre = "Alaska" };

      //  primero podemos probar ingresar una localidad sin provincia

      ctx.Localidades.Add(nuevaLocalidad);
      try
      {
        ctx.SaveChanges();

        throw new ApplicationException("Deberia haber fallado el UPDATE!!!");
      }
      catch (DbUpdateException ex)
      {
        Console.WriteLine("Se genero UpdateException!! Todo funciona OK");
        Console.WriteLine(ex.Message);
      }

      nuevaLocalidad.Provincia = nuevaProvincia;
      ctx.SaveChanges();
#endif

#if PASO_5
      //  PASO 5 - Ingreso de algunas personas
      //
      Persona newPersona;
      Localidad rosario, perez;

      rosario = ctx.Localidades.Where(loc => loc.Nombre == "Rosario" && loc.Provincia.Nombre == "Santa Fe").FirstOrDefault();
      perez = ctx.Localidades.Where(loc => loc.Nombre == "Perez" && loc.Provincia.Nombre == "Santa Fe").FirstOrDefault();


      newPersona = new Persona() { Nombres = "Martin", Apellidos = "Vidal", Localidad = rosario, Sexo = Sexo.Masculino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Gonzalo", Apellidos = "López", Localidad = rosario, Sexo = Sexo.Masculino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Ruben", Apellidos = "Acevedo", Localidad = rosario, Sexo = Sexo.Masculino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona()
      {
        Nombres = "Enrique",
        Apellidos = "Thedy",
        Localidad = rosario,
        Domicilio = "Mitre 509 Piso 5 Departamento 4",
        CodigoPostal = "S2000COK", 
        Documento = "18339577",
        TipoDocumento = ctx.TiposDeDocumento.FirstOrDefault(td => td.Descripcion == "DNI"),
        Sexo = Sexo.Masculino
      };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Ana Laura", Apellidos = "Cardoso", Localidad = rosario, Sexo = Sexo.Femenino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Maria Elisa", Apellidos = "Tron", Localidad = rosario, Sexo = Sexo.Femenino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Valeria Claudia", Apellidos = "Guglielmetti", Localidad = rosario, Sexo = Sexo.Femenino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Guillermo", Apellidos = "Quintana", Localidad = rosario, Sexo = Sexo.Masculino };
      ctx.Personas.Add(newPersona);

      newPersona = new Persona() { Nombres = "Irene Leonor", Apellidos = "Acosta", Localidad = perez, Sexo = Sexo.Femenino };
      ctx.Personas.Add(newPersona);

      //  newPersona = new Persona() { Nombres = "", Apellidos = "", Localidad = rosario };

      ctx.SaveChanges();
#endif

#if PASO_5_5
      //  Modificar propiedades de alguna persona
      Persona persona = ctx.Personas.Where(per => per.Apellidos == "Thedy").First();

      persona.AmpliacionDomicilio = "Es el edificio de la esquina";

      if (ctx.ChangeTracker.HasChanges())
      {
        Console.WriteLine("Guardando info de contacto");

        ctx.MostrarCambios();
        Console.ReadLine();

        ctx.SaveChanges();
      }
      else
      {
        Console.WriteLine("No se detectaron cambios");
      }
#endif

#if PASO_6
      //  Agregamos info de contacto
      //
      //  TODO Traer una Persona desde la base de datos
      Persona persona = ctx.Personas.Where(per => per.Apellidos == "Thedy").FirstOrDefault() ;     
      List<TipoContacto> tipos = ctx.TiposContacto.ToList();

      if (persona != null)
      {
        Console.WriteLine($"Agregar contactos para {persona.Nombres} {persona.Apellidos}");

        while (true)
        {
          Console.WriteLine("(A)gregar nuevo contacto? o (S)alir?");
          if (Console.ReadKey(true).Key == ConsoleKey.A)
          {
            //  mostrar la lista de posibles contactos
            Console.WriteLine("Esta es la lista de posibles tipos de contacto\nElegir un # de tipo y luego ingresarlo");

            for (int idx = 1; idx <= tipos.Count ; idx++)
              Console.WriteLine($"{idx} ----- {tipos[idx-1].Descripcion}");

            string opcion = Console.ReadLine();
            int numOpcion;

            if (Int32.TryParse(opcion, out numOpcion) && numOpcion >= 1 && numOpcion <= tipos.Count)
            {
              Console.WriteLine("Ahora ingrese el dato del contacto");

              string dato = Console.ReadLine();

              Console.WriteLine("Por ultimo un comentario (o Enter si no desea comentario)");

              string comentario = Console.ReadLine();

              //  TODO Agregar el nuevo contacto a la Persona
              Contacto nuevoContacto = new Contacto();

              nuevoContacto.Dato = dato;
              nuevoContacto.Comentario = string.IsNullOrWhiteSpace(comentario) ? null : comentario;
              nuevoContacto.Tipo = tipos[numOpcion];

              persona.InfoContacto.Add(nuevoContacto);
            }
            else
            {
              Console.WriteLine($"Opcion no valida, debe estar entre 1 y {tipos.Count}");
            }
          }
          else
          {
            break;
          }
        }
        if (ctx.ChangeTracker.HasChanges())
        {
          Console.WriteLine("Guardando info de contacto");

          ctx.MostrarCambios();
          Console.ReadLine();

          ctx.SaveChanges();
        }
        else
        {
          Console.WriteLine("No se detectaron cambios");
        }
      }
      else
      {
        Console.WriteLine("Esta persona no existe");
      }
#endif

#if PASO_7
      //  Agregamos Empleado asociado a Persona
      //
      Persona persona = ctx.Personas.FirstOrDefault(per => per.Apellidos == "Thedy");

      if (persona != null)
      {
        Empleado nuevo = new Empleado();

        nuevo.Persona = persona;
        nuevo.Legajo = "167055";
        nuevo.CUIT = "20-18339577-8";
        nuevo.FechaIngreso = new DateTime(1986, 12, 9);

        ctx.Empleados.Add(nuevo);
        ctx.SaveChanges();
      }
#endif

#if PASO_8
      //  Creamos un usuario y utilizamos el servicio para crearlo con su password
      //
      Empleado empleado = ctx.Empleados.FirstOrDefault(emp => emp.Legajo == "167055");
      Usuario user = new Usuario();
      SecurityServices seg = new SecurityServices();

      user.Login = "ethedy1";
      user.Empleado = empleado;
      user.Blocked = false;

      if (seg.CrearUsuario(user, "12345678"))
      {
        Console.WriteLine("Usuario creado correctamente");
      }
      else
      {
        Console.WriteLine("Error al crear el usuario");
      }

#endif

#if PASO_9
      //  TODO agregar codigo para ingresar con el ID y password que asignamos a nuestro usuario
      //
      SecurityServices seg = new SecurityServices();

      //  Probamos login incorrecto...por usuario inexistente
      //
      Usuario usrlogin = seg.LoginUsuario("pirulo", "12345678");

      if (usrlogin == null)
        Console.WriteLine(seg.ErrorInfo);
      else
      {
        Console.WriteLine("NO PUEDE SER!!!");
      }
      //  Probamos login incorrecto...por password incorrecta ==> esto tiene que modificar la tabla (last login BAD))
      //
      usrlogin = seg.LoginUsuario("ethedy", "1234567890");

      if (usrlogin == null)
        Console.WriteLine(seg.ErrorInfo);
      else
      {
        Console.WriteLine("NO PUEDE SER!!!");
      }

      //  Probamos login incorrecto...por password incorrecta ==> esto tiene que modificar la tabla (last login OK)
      //
      usrlogin = seg.LoginUsuario("ethedy", "12345678");

      if (usrlogin != null)
        Console.WriteLine($"Usuario {usrlogin.Empleado.Persona.Nombres} {usrlogin.Empleado.Persona.Apellidos} conectado!!");
      else
      {
        Console.WriteLine("NO PUEDE SER!!!");
      }
#endif

#if PASO_12
      //  Agregamos codigo para incorporar editoriales y algunos libros
      //  string pathPortadas = @"F:\CURSO_2016_01\src\OMB\imagenes\portadas";
      //  string pathPortadas = @"F:\CURSO_2016_01\src\OMB\imagenes\portadas";
      string[] editoriales = {
                               // "Addison-Wesley Professional",
                               "The MIT Press",
                               "O'Reilly Media",
                               "No Starch Press",
                               "Apress",
                               "Microsoft Press",
                               "Wrox",
                               "McGraw-Hill Education",
                               "Wiley"
                             };

      Editorial editorial;
      //  ProductServices prods = new ProductServices();

      foreach (string item in editoriales)
      {
        editorial = new Editorial() { Nombre = item };
        ctx.Editoriales.Add(editorial);
      }
      ctx.SaveChanges();
/*
      Libro nuevo = prods.NuevoLibro(editorial, 
        "The C++ Standard Library: A Tutorial and Reference", "9780321623218", 
        Path.Combine(pathPortadas, "The C++ Standard Library A Tutorial and Reference.jpg"),
        new DateTime(2012, 4, 9), 1128);

      if (nuevo != null)
        Console.WriteLine($"Libro agregado correctamente con ID = {nuevo.IDLibro}");
 * */
#endif

      /*
            Categoria newCat = new Categoria() {Nombre = "Informatica", Parent = parent};

            ctx.Categorias.Add(newCat);

            parent.SubCategorias.Add(newCat);
            ctx.SaveChanges();


            //  ctx.Categorias.Load();

            var showCat = ctx.Categorias.FirstOrDefault();

              //.Where(categ => categ.Nombre == "Libros").First();

            Console.WriteLine(showCat?.CategoriaID.ToString());
      */
      Console.WriteLine("Press Any Key To Continue...");
      Console.ReadLine();

      ctx.Dispose();
    }
  }
}
