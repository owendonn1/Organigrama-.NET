# ERP-Básico 📖

## Objetivos 📋
Desarrollar un sistema, que permita la administración general de un consultorio (de cara a los administradores): Prestaciones, Profesionales, Pacientes, etc., como así también, permitir a los pacientes, realizar reserva sobre turnos ofrecidos.
Utilizar Visual Studio 2022 preferentemente y crear una aplicación utilizando ASP.NET MVC Core (versión a definir por el docente ).

<hr />

## Enunciado 📢
La idea principal de este trabajo práctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relevó e identificó la información aquí contenida. 
A partir de este momento, deberán comprender lo que se está requiriendo y construir dicha aplicación web.

Lo  primero debra ser comprender al detalle, que es lo que se espera y busca del proyecto, para ello, deben recopilar todas las dudas que tengan entre Uds. y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, él nos ayudará a conseguir la información ya un poco más procesada. 
Es importante destacar, que este proceso no debe esperar a hacerlo en clase; deben ir contemplandolas independientemente, las unifican y hace una puesta comun dentro del equipo, ya sean de índole funcional o técnicas, en lugar de que cada consulta enviarla de forma independiente, se recomienda que las envien de manera conjunta. 

Al inicio del proyecto, las consultas seran realizadas por correo y deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>
2.< xxxxxxxx>

# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relación del paciente con Turno es 1:1 o 1:N?
2.Está bien que encaremos la validación del turno activo, con una propiedad booleana en el Turno?

<hr />

Es sumamente importante que los correos siempre tengan:
1.Subject con la referencia, para agilizar cualquier interaccion entre el docente y el grupo
2. Siempre que envien una duda o consulta, pongan en copia a todos los participantes del equipo. 

Nota: A medida que avancemos en la materia, las dudas seran canalizadas por medio de Github, y alli tendremos las dudas comentadas, accesibles por todos y el avance de las mismas. 

**Crear un Issue o escribir un nuevo comentario sobre el issue** que se requiere asistencia, siempre arrobando al docente, ejemplo: @marianolongoort


### Proceso de ejecución en alto nivel ☑️
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/) utilizando la template de MVC.
 - Crear todos los modelos definidos y/o detectados por ustedes, dentro de la carpeta Models cada uno en un archivo separado (Modelos anemicos).
 - En el proyecto encararemos y permitiremos solo una herencia entre los modelos anemicos. Comforme avancemos, veremos que en este nivel, que estos modelos tengan una herencia, sera visto como una mala practica, pero es la mejor forma de visualizarlo. Esta unica herencia soportada sera PERSONA como clase base y luego diferentes especializaciones, segun sea el proyecto (Cliente, Alumno, Profesional, etc.).  
 - Sobre dichos modelos, definir y aplica las restricciones necesarias y solicitadas para cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Agregar las propiedades navegacionales, sobre las relaciones entre las entidades (modelos).
 - Agregar las propiedades relacionales, en el modelo donde se quiere alojar la relacion (entidad dependiente).
 - Crear una carpeta Data que dentro tendrá al menos la clase que representará el contexto de la base de datos (DbContext) en nuestra aplicacion. 
 - Agregar los paquetes necesarios para Incorporar Entity Framework e Identitiy en nuestros proyectos.
 - Crear el DbContext utilizando en esta primera estapa con base de datos en memoria (con fines de testing inicial, introduccion y fine tunning de las relaciones entre modelos). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades que queremos persistir en el DbContext.
 - Agregar Identity a nuestro poryecto, para facilitar la inclusion de funcionalidades como Iniciar y cerrar sesion, agregado de entidades de soporte para esto Usuario y Roles que nos serviran para aplicar un control de acceso basado en roles (RBAC) basico. 
 - Por medio de Scaffolding, crear en esta instancia todos los CRUD de las entidades a persistir. Luego verificaremos que se mantiene, que se remueve, que se modifica y que debemos agregar.
 - Antes de continuar es importante realizar algun tipo de pre-carga de la base de datos. No solo es requisito del proyecto, sino que les ahorrara mucho tiempo en las pruebas y adecuaciones de los ABM.
 - Testear en detalle los ABM generado, y detectar todas las modificaciones requeridas para nuestros ABM e interfaces de usuario faltantes para resolver funcionalidades requeridas. (siempre tener presente el checklist de evaluacion final, que les dara el rumbo para esto).
 - Cambiar el dabatabase service provider de Database In Memory a SQL. Para aquellos casos que algunos alumnos utilicen MAC, tendran dos opciones para avanzar (adecuar el proyecto, para utilizar SQLLite o usar un docker con SQL Server instalado alli).
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Si el proyecto lo requiere, generar el proceso de auto-registración. Es importante aclarar que este proceso estara supeditado a las necesidades de cada proyecto y seguramente para una entidad especifica. 
 - A estas alturas, ya se han topado con varios inconvenientes en los procesos de adecuacion de las vistas y por consiguiente es una buena idea que generen ViewModels para desbloquear esas problematicas que nos estan trayendo los Modelos anemicos utilizados hasta el momento.
 - En el caso de ser requerido en el enunciado, un administrador podrá realizar todas tareas que impliquen interacción del lado del negocio (ABM "Alta-Baja-Modificación" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente o equivalente> sólo podrá tomar acción en el sistema, en base al rol que que se le ha asignado al momento de auto-registrarse o creado por otro medio o entidad.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos desde la perspectiva de permisos y validaciones.
 - Todo lo referido a la presentación de la aplicaión (cuestiones visuales).
 
 Nota: Para la pre-carga de datos, las cuentas creadas por este proceso, deben cumplir las siguientes reglas:
 1. La contraseña por defecto para todas las cuentas pre-cargadas será: Password1!
 2. El UserName y el Email deben seguir la siguiente regla:  <classname>+<rolname si corresponde diferenciar>+<indice>@ort.edu.ar Ej.: cliente1@ort.edu.ar, empleado1@ort.edu.ar, empleadorrhh1@ort.edu.ar

<hr />

## Entidades 📄

- Persona
- Empleado
- Telefono
- TipoTelefono
- Foto
- Posición
- Gerencia
- CentroDeCosto
- Gastos
- Empresa


`Importante: Todas las entidades deben tener su identificador unico. Id`

`
Las propiedades descriptas a continuación, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como así también las restricciones.
`

**Persona**
```
- UserName
- Password
- Email
- FechaAlta
```

**Empleado**
```
- Nombre
- Apellido
- DNI
- Telefonos
- Direccion
- FechaAlta
- Email 
- ObraSocial
- Legajo
- EmpleadoActivo
- Posicion
- Foto
```
**Telefono**
```
- Numero
- TipoTelefono
```

**TipoTelefono**
```
- Nombre
```

**Imagen**
```
- Nombre
- Path
```

**Posicion**
```
- Nombre
- Descipcion
- Sueldo
- Empleado
- Responsable(Jefe)
- Gerencia
```

**Gerencia**
```
- Nombre
- EsGerenciaGeneral
- Direccion(Gerencia)
- Responsable(Posicion)
- Posiciones
- Gerencias
- Empresa
```

**CentroDeCosto**
```
- Nombre
- MontoMaximo
- Gastos
```

**Gastos**
```
- Descipcion
- CentroDeCosto
- Empleado
- Monto
- Fecha
```

**Empresa**
```
- Nombre
- Rubro
- Logo
- Direccion
- TelefonoContacto
- EmailContacto
```

**NOTA:** aquí un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades ⌨️
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

`IMPORTANTE: Solo empleados de RRHH pueden realizar modificaciones en la nomina de Empleados, Pero todos pueden ver información general de la Empresa.`

**General**
- Los Empleados no pueden auto-registrarse.

**Empleado**
- Los empleados, deben ser agregados por otro Empleado.
	- Al momento, del alta del empleado, se le definirá un username y password definida automaticamente por el sistema con el DNI.
    - También se le asignará a estas cuentas el rol de empleado y adicionalmente el de RRHH si corresponde.
- Un empleado puede consultar todos sus datos y actualizar solo sus telefonos y foto.
- Puede navegar el organigrama. Para detalle, de funcionalidad, ver Organigrama. 
- Puede actualizar datos de contacto, como sus telefonos, dirección y foto.. Pero no puede modificar su DNI, Nombre, Apellido, etc.
- Pueden Imputar gastos a su Centro de Costo.
- Cada empleado puede listar sus gastos en orden decreciente, por fecha.

**Organigrama**
- La visualización del Organigrama, siempre estará disponible para cualquier Empleado de la compañia. 
- Se parte por la Gerencia General (solo puede existir una), se visualiza el Responsable de la misma.
- Al hacer click, en ella, se visualizará, el siguiente nivel de jerarquico, con cada una de las gerencias que la componga y sus correspondientes responsables.
    - En el caso de ser una Gerencia, que no tiene subGerencias, mostrar todos los empleados que la componen (Solo Apellido y Nombre), en formado de lista, con orden Ascendente con la condición Apellido y Nombre.
    - Se podrá hacer click en cada empleado, para visualizar una tarjeta de contacto.
    - La tarjeta de contacto tendrá: Apellido, Nombre, Nombre de la posición, Telefonos e Email.
    Importante: Solo se visualizarán empleados que estén activos en nomina. 

**Empleado de RRHH**
- Un empleado de RRHH, puede crear, modificar todos los datos de los Empresa, otros empleados, Gerencias, Posiciones, etc.. Tiene control total de la información contenida en el sistema.
    - No puede eliminar Empleados, pero si puede deshabilitarlos, para indicar que no son empleados actuales de nomina.
- El Empleado puede listar todos los empleado, y por cada uno, ver en sus detalles, como así también, realizar modificaciones. 
- Puede realizar un listado de empleados, con sueldo en forma decreciente por el concepto de sueldos y luego Creciente para Apellido y Nombre. 
    - Se debe incluir Sueldo,Apellido, Nombre, Nombre de la posición.
- No puede modificar ni eliminar un Gasto de cualquier Empleado.
- Puede listar todos los gastos de todos los empleados en forma decreciente por fecha y ordenados de manera creciente por Apellido y Nombre. Se debe incluir la información de que gerencia pertenece.
- Listar los montos totales de gastos por cada gerencia, idependientemente de si es una Gerencia o SubGerencia. (Orden decreciente por monto)

**Posición**
- La posición, es la que dará la relación entre un Empleado y la Gerencia a la cual pertenece.
- La posición, puede o no tener empleados actualmente ocupandola. Ej. La posición de gerente de IT, puede estar disponible, sin un empleado que la esté ocupando.
- El Responsable de dicha posición, es otra posición, no es un empleado. En todo caso, tiene una Posición de la cual depende, y esa otra posición tiene un Empleado o no que está ocupando esa posición.
- La posición es la que tiene un sueldo designado, no es el empleado.
- Pertenece a una Gerencia.


**Centro de Costo**
- El centro de Costos puede ser asociado a solo una gerencia de cualquier nivel.
- Los empleados, solo pueden imputar gastos al centro de costo que tenga la gerencia a la cual pertenecen de forma directa.
- Se pueden imputar gastos al centro de costo, dejando registro de cual es el empleado que lo está imputando, siempre y cuando, no supere el monto maximo del Centro de Costo.


**Aplicación General**
- Información institucional, en base a la información de la Empresa, con su respectiva imagen (Logo)
- No existe un limite para las posiciones dependientes. Ejemplo, puede existir 10 o 1000 posiciones que dependan de una posición gerencial. En otras palabras, un Gerente asignado a una posición de Gerente de IT, puede tener 1, 10 o 1000 posiciones de IT general, que dependen de ella. 
- Los accesos a las funcionalidades y/o capacidades, debe estar basada en los roles que tenga cada individuo.


