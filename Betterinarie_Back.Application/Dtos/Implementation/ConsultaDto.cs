using Betterinarie_Back.Core.Entities.Implementation.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betterinarie_Back.Application.Dtos.Implementation
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly Hora { get; set; }

        public EstatusConsulta Estatus { get; set; }
        public string EstatusNombre { get; set; }
        public string Motivo { get; set; }
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public List<int> MedicamentosIds { get; set; }
    }

    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string TimeFormat = "HH:mm:ss";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString(), TimeFormat);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat));
        }
    }
}
