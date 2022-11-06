using System;
using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models;

public sealed class Position
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("id_type")] public int IdType { get; set; }
    // Рабочий телефон
    [JsonPropertyName("phone")] public string Phone { get; set; } = string.Empty;
    // Навзание должности
    [JsonPropertyName("position")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type_contract")] public string Contract { get; set; } = string.Empty;

    [JsonPropertyName("data_start_contract")] public DateTime? DateStartContract { get; set; }

    [JsonPropertyName("data_end_contract")] public DateTime? DateEndContract { get; set; }
    //TODO: Потом поменяй на объект в API чтобы можно было через combobox выбирать отдел
    [JsonPropertyName("name_depart")] public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("id_order")] public int IdOrder { get; set; }

    [JsonPropertyName("order")] public string Order { get; set; } = string.Empty;

    [JsonPropertyName("date_order")]
    public DateTime? DateOrder { get; set; }

    [JsonPropertyName("date_drop")]
    public DateTime? DateDrop { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? DateCreate { get; set; }

    [JsonPropertyName("fullname_decree")] public string NameDecree { get; set; } = string.Empty;

    [JsonPropertyName("id_depart")] public int IdDepartment { get; set; } = 0;

    // Является ли должность педагогической
    [JsonPropertyName("is_ped")] public bool IsPed { get; set; }

    // Основная ли должность
    [JsonPropertyName("is_main")] public bool IsMain { get; set; }

    // Сколько дней отпуска по должности
    [JsonPropertyName("holiday")] public int? HolidayLimit { get; set; }

    [JsonPropertyName("stavka_nobudget")] public decimal StavkaNoBudget { get; set; }

    [JsonPropertyName("stavka_budget")] public decimal StavkaBudget { get; set; }

    public decimal StavkaAll { get => StavkaBudget + StavkaNoBudget; set => StavkaAll = value; }

    private decimal _CountAllBudget;

    public decimal CountAllBudget
    {
        get => _CountAllBudget = Count_B + Count_NB;
        set => _CountAllBudget = value;
    }

    private decimal _CountAllFreeBudget;

    public decimal CountAllFreeBudget
    {
        get => _CountAllFreeBudget = Free_B + Free_NB;
        set => _CountAllFreeBudget = value;
    }

    private decimal _CountOklad;

    public decimal CountOklad
    {
        get => _CountOklad = Oklad_B + Oklad_NB;
        set => _CountOklad = value;
    }

    [JsonPropertyName("count_budget")] public decimal Count_B { get; set; }

    [JsonPropertyName("count_nobudget")] public decimal Count_NB { get; set; } 

    [JsonPropertyName("free_budget")]  public decimal Free_B { get; set; }

    [JsonPropertyName("free_nobudget")] public decimal Free_NB { get; set; }

    [JsonPropertyName("oklad_budget")] public decimal Oklad_B { get; set; }

    [JsonPropertyName("oklad_nobudget")] public decimal Oklad_NB { get; set; }

    [JsonPropertyName("priority")] public short Priority { get; set; }

    [JsonPropertyName("name_city_job")] public string Place { get; set; } = string.Empty;

    [JsonPropertyName("id_contract")] public int IdContract { get; set; }

    [JsonPropertyName("is_pluralism_oter")] public bool IsPluralismOter { get; set; }
}