﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyBot.Services.JsonConverters
{
    internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "dd.MM.yyyy";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString()!;
            return DateOnly.ParseExact(dateString, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
