using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Receitas_API.Models
{
    public partial class RecipeInfo
    {
        [JsonProperty("vegetarian")]
        public bool Vegetarian { get; set; }

        [JsonProperty("vegan")]
        public bool Vegan { get; set; }

        [JsonProperty("glutenFree")]
        public bool GlutenFree { get; set; }

        [JsonProperty("dairyFree")]
        public bool DairyFree { get; set; }

        [JsonProperty("veryHealthy")]
        public bool VeryHealthy { get; set; }

        [JsonProperty("cheap")]
        public bool Cheap { get; set; }

        [JsonProperty("veryPopular")]
        public bool VeryPopular { get; set; }

        [JsonProperty("sustainable")]
        public bool Sustainable { get; set; }

        [JsonProperty("weightWatcherSmartPoints")]
        public long WeightWatcherSmartPoints { get; set; }

        [JsonProperty("gaps")]
        public string Gaps { get; set; }

        [JsonProperty("lowFodmap")]
        public bool LowFodmap { get; set; }

        [JsonProperty("aggregateLikes")]
        public long AggregateLikes { get; set; }

        [JsonProperty("spoonacularScore")]
        public long SpoonacularScore { get; set; }

        [JsonProperty("healthScore")]
        public long HealthScore { get; set; }

        [JsonProperty("pricePerServing")]
        public double PricePerServing { get; set; }

        [JsonProperty("extendedIngredients")]
        public ExtendedIngredient[] ExtendedIngredients { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("readyInMinutes")]
        public long ReadyInMinutes { get; set; }

        [JsonProperty("servings")]
        public long Servings { get; set; }

        [JsonProperty("sourceUrl")]
        public Uri SourceUrl { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("imageType")]
        public string ImageType { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("cuisines")]
        public string[] Cuisines { get; set; }

        [JsonProperty("dishTypes")]
        public string[] DishTypes { get; set; }

        [JsonProperty("diets")]
        public string[] Diets { get; set; }

        [JsonProperty("occasions")]
        public object[] Occasions { get; set; }

        [JsonProperty("winePairing")]
        public WinePairing WinePairing { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("analyzedInstructions")]
        public AnalyzedInstruction[] AnalyzedInstructions { get; set; }

        [JsonProperty("sourceName")]
        public object SourceName { get; set; }

        [JsonProperty("creditsText")]
        public object CreditsText { get; set; }

        [JsonProperty("originalId")]
        public object OriginalId { get; set; }
    }

    public partial class AnalyzedInstruction
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("steps")]
        public Step[] Steps { get; set; }
    }

    public partial class Step
    {
        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("step")]
        public string StepStep { get; set; }

        [JsonProperty("ingredients")]
        public Ent[] Ingredients { get; set; }

        [JsonProperty("equipment")]
        public Ent[] Equipment { get; set; }

        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        public Length Length { get; set; }
    }

    public partial class Ent
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("localizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public Length Temperature { get; set; }
    }

    public partial class Length
    {
        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public partial class ExtendedIngredient
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("aisle")]
        public Aisle Aisle { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("consistency")]
        public Consistency Consistency { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }

        [JsonProperty("originalString")]
        public string OriginalString { get; set; }

        [JsonProperty("originalName")]
        public string OriginalName { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("meta")]
        public string[] Meta { get; set; }

        [JsonProperty("metaInformation")]
        public string[] MetaInformation { get; set; }

        [JsonProperty("measures")]
        public Measures Measures { get; set; }
    }

    public partial class Measures
    {
        [JsonProperty("us")]
        public Metric Us { get; set; }

        [JsonProperty("metric")]
        public Metric Metric { get; set; }
    }

    public partial class Metric
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("unitShort")]
        public string UnitShort { get; set; }

        [JsonProperty("unitLong")]
        public string UnitLong { get; set; }
    }

    public partial class WinePairing
    {
    }

    public enum Aisle { OilVinegarSaladDressing, Produce, SpicesAndSeasonings };

    public enum Consistency { Liquid, Solid };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AisleConverter.Singleton,
                ConsistencyConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AisleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Aisle) || t == typeof(Aisle?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Oil, Vinegar, Salad Dressing":
                    return Aisle.OilVinegarSaladDressing;
                case "Produce":
                    return Aisle.Produce;
                case "Spices and Seasonings":
                    return Aisle.SpicesAndSeasonings;
            }
            throw new Exception("Cannot unmarshal type Aisle");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Aisle)untypedValue;
            switch (value)
            {
                case Aisle.OilVinegarSaladDressing:
                    serializer.Serialize(writer, "Oil, Vinegar, Salad Dressing");
                    return;
                case Aisle.Produce:
                    serializer.Serialize(writer, "Produce");
                    return;
                case Aisle.SpicesAndSeasonings:
                    serializer.Serialize(writer, "Spices and Seasonings");
                    return;
            }
            throw new Exception("Cannot marshal type Aisle");
        }

        public static readonly AisleConverter Singleton = new AisleConverter();
    }

    internal class ConsistencyConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Consistency) || t == typeof(Consistency?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "liquid":
                    return Consistency.Liquid;
                case "solid":
                    return Consistency.Solid;
            }
            throw new Exception("Cannot unmarshal type Consistency");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Consistency)untypedValue;
            switch (value)
            {
                case Consistency.Liquid:
                    serializer.Serialize(writer, "liquid");
                    return;
                case Consistency.Solid:
                    serializer.Serialize(writer, "solid");
                    return;
            }
            throw new Exception("Cannot marshal type Consistency");
        }

        public static readonly ConsistencyConverter Singleton = new ConsistencyConverter();
    }
}
