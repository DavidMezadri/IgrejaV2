using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IgrejaV2.Dominio.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IgrejaV2.API.Servicos;

public class TokenServico(IConfiguration config)
{
    public (string Token, DateTime Expiracao) Gerar(Usuario usuario)
    {
        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Chave"]!));

        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var expiracao = DateTime.UtcNow.AddHours(
            int.Parse(config["Jwt:ExpiracaoHoras"] ?? "8"));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NomeUsuario),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim("tipo_usuario", usuario.TipoUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Emissor"],
            audience: config["Jwt:Audiencia"],
            claims: claims,
            expires: expiracao,
            signingCredentials: credenciais);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiracao);
    }
}
