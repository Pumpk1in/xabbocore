using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using Xabbo.Core.Serialization;

namespace Xabbo.Core.GameData.Json;

public class FigureData
{
    public static FigureData Load(string json)
    {
        var container = JsonSerializer.Deserialize(json, GameDataJsonContext.Default.FigureDataContainer)
            ?? throw new Exception("Failed to deserialize figure data.");
        return container.FigureData;
    }

    public static FigureData LoadFile(string path) => Load(File.ReadAllText(path));

    [JsonPropertyName("sets")]
    public PaletteSets Sets { get; set; } = new();

    [JsonPropertyName("colors")]
    public ColorPalettes Colors { get; set; } = new();

    public class PaletteSets
    {
        [JsonPropertyName("settype")]
        public List<PartSetCollection> SetCollections { get; set; } = [];
    }

    public class ColorPalettes
    {
        [JsonPropertyName("palette")]
        public List<Palette> Palettes { get; set; } = [];
    }

    public class Palette
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("color")]
        public List<Color> Colors { get; set; } = [];
    }

    public class Color
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("club")]
        public int RequiredClubLevel { get; set; }

        [JsonPropertyName("selectable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsSelectable { get; set; }

        [JsonPropertyName("content")]
        [JsonConverter(typeof(StringConverter))]
        public string Value { get; set; } = "";
    }

    public class PartSetCollection
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("paletteid")]
        public int PaletteId { get; set; }

        [JsonPropertyName("mand_m_0")]
        public int mand_m_0 { get; set; }

        [JsonPropertyName("mand_f_0")]
        public int mand_f_0 { get; set; }

        [JsonPropertyName("mand_m_1")]
        public int mand_m_1 { get; set; }

        [JsonPropertyName("mand_f_1")]
        public int mand_f_1 { get; set; }

        [JsonPropertyName("set")]
        [JsonConverter(typeof(SingleOrArrayConverter<PartSet>))]
        public List<PartSet> Sets { get; set; } = [];
    }

    public class PartSet
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = "";

        [JsonPropertyName("club")]
        public int RequiredClubLevel { get; set; }

        [JsonPropertyName("colorable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsColorable { get; set; }

        [JsonPropertyName("selectable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsSelectable { get; set; }

        [JsonPropertyName("preselectable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsPreSelectable { get; set; }

        [JsonPropertyName("sellable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsSellable { get; set; }

        [JsonPropertyName("part")]
        [JsonConverter(typeof(SingleOrArrayConverter<Part>))]
        public List<Part> Parts { get; set; } = [];
    }

    public class Part
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("colorable")]
        [JsonConverter(typeof(IntBoolConverter))]
        public bool IsColorable { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("colorindex")]
        public int ColorIndex { get; set; }
    }
}

public class FigureDataContainer
{
    [JsonPropertyName("figuredata")]
    public FigureData FigureData { get; set; } = new();
}
