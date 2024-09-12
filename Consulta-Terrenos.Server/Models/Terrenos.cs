namespace Consulta_Terrenos.Server.Models
{
    /// <summary>
    /// Representa um terreno disponível para consulta e cadastro.
    /// </summary>
    public class Terrenos
    {
        /// <summary>
        /// Identificador único do terreno.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordenadas geográficas do terreno (em formato de string, ex.: "latitude,longitude") ou nome da localidade.
        /// </summary>
        public string? Coordenadas { get; set; }

        /// <summary>
        /// Tamanho do terreno em metros quadrados.
        /// </summary>
        public double Tamanho { get; set; }

        /// <summary>
        /// Preço do terreno.
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// Tipo de uso do terreno (ex.: Residencial, Comercial).
        /// </summary>
        public string? TipoUso { get; set; }
    }
}

