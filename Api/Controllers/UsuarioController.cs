using Api.Models.Request;
using CapaNegocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        // Es como un "acceso" que le das a tu controlador para que pueda leer la configuración de la aplicación, especialmente la sección "Jwt" que definiste en appsettings.json
        public UsuarioController(IConfiguration config)
        {
            //Constructor que recibe la configuración mediante inyección de dependencias.
            _config = config;
            //Asigna el parámetro al campo privado para usarlo después.
        }


        [HttpPost("login")]
        public IActionResult ValidarUsuario([FromBody] LoginRequest usuario)
        {
            CN_Usuario ObjetoCN = new CN_Usuario();
            if (ObjetoCN.ValidarUsuario(usuario.Usuario, usuario.Password))
            {
                //Si las credenciales son correctas, generamos el Token
                var tokenString = GenerarTokenJWT(usuario.Usuario);
                // Retornamos el Token al cliente
                return Ok(new
                {
                    mensaje = "Bienvenido",
                    usuario = usuario.Usuario,
                    token = tokenString // <-- El frontend guardará esto
                });
            }
            return Unauthorized();
        }


        [HttpPost("RegistrarUsuario")]//este endpoint nos sirve para crear/ registrar un nuevo usuario para poder loguearnos
        public IActionResult RegistrarUsuario([FromBody] RegistrarUsuarioRequest Usuario)
        {
            CN_Usuario ObjetoCN = new CN_Usuario();

            if (ObjetoCN.RegistrarUsuario(Usuario.Usuario, Usuario.Password))
            {
                return Ok(new{
                    mensaje = "Usuario resgistrado Correctamente."
                });
            }
            return BadRequest(
                new
                {
                    mensaje = "No se a podido registrar el Usuario."
                });
            
        }



        private string GenerarTokenJWT(string usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            // Solo el usuario, sin roles inventados
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, usuario)
    };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
