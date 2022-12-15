using System;
using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaDesktop.Models;

public sealed class Position : ReactiveObject
{
    [JsonPropertyName("id")] [Reactive] public int Id { get; set; }

    [JsonPropertyName("id_type")] [Reactive] public int IdType { get; set; }
    // Рабочий телефон
    [JsonPropertyName("phone")] [Reactive] public string Phone { get; set; } = string.Empty;
    // Навзание должности
    [JsonPropertyName("position")] [Reactive] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type_contract")] [Reactive] public string Contract { get; set; } = string.Empty;

    [JsonPropertyName("data_start_contract")] [Reactive] public DateTime? DateStartContract { get; set; }

    [JsonPropertyName("data_end_contract")] [Reactive]  public DateTime? DateEndContract { get; set; }
    [JsonPropertyName("name_depart")] [Reactive] public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("id_order")] [Reactive] public int IdOrder { get; set; }

    [JsonPropertyName("order")] [Reactive] public string Order { get; set; } = string.Empty;

    [JsonPropertyName("date_order")]
    [Reactive] 
    public DateTime? DateOrder { get; set; }

    [JsonPropertyName("date_drop")]
    [Reactive] 
    public DateTime? DateDrop { get; set; }

    [JsonPropertyName("created_at")]
    [Reactive] 
    public DateTime? DateCreate { get; set; }

    [JsonPropertyName("fullname_decree")] [Reactive] public string NameDecree { get; set; } = string.Empty;

    [JsonPropertyName("id_depart")] [Reactive] public int IdDepartment { get; set; } = 0;

    // Является ли должность педагогической
    [JsonPropertyName("is_ped")] [Reactive] public bool IsPed { get; set; }

    // Основная ли должность
    [JsonPropertyName("is_main")] [Reactive] public bool IsMain { get; set; }

    // Сколько дней отпуска по должности
    [JsonPropertyName("holiday")] [Reactive] public int? HolidayLimit { get; set; }

    [JsonPropertyName("stavka_nobudget")] [Reactive] public decimal StavkaNoBudget { get; set; }

    [JsonPropertyName("stavka_budget")] [Reactive] public decimal StavkaBudget { get; set; }

     public decimal StavkaAll => StavkaBudget + StavkaNoBudget;

     private decimal _countAllBudget;

    public decimal CountAllBudget
    {
        get => _countAllBudget = Count_B + Count_NB;
        set => this.RaiseAndSetIfChanged(ref _countAllBudget, value);
    }

    private decimal _countAllFreeBudget;

    public decimal CountAllFreeBudget
    {
        get => _countAllFreeBudget = Free_B + Free_NB;
        set => this.RaiseAndSetIfChanged(ref _countAllFreeBudget, value);
    }

    private decimal _countOklad;

    public decimal CountOklad
    {
        get => _countOklad = Free_B + Free_NB;
        set => this.RaiseAndSetIfChanged(ref _countOklad, value);
    }

    [JsonPropertyName("count_budget")] [Reactive] public decimal Count_B { get; set; }

    [JsonPropertyName("count_nobudget")] [Reactive] public decimal Count_NB { get; set; } 

    [JsonPropertyName("free_budget")]  [Reactive] public decimal Free_B { get; set; }

    [JsonPropertyName("free_nobudget")] [Reactive] public decimal Free_NB { get; set; }

    [JsonPropertyName("oklad_budget")] [Reactive] public decimal Oklad_B { get; set; }

    [JsonPropertyName("oklad_nobudget")] [Reactive] public decimal Oklad_NB { get; set; }

    [JsonPropertyName("priority")] [Reactive] public short Priority { get; set; }

    [JsonPropertyName("name_city_job")] [Reactive] public string Place { get; set; } = string.Empty;

    [JsonPropertyName("id_contract")] [Reactive] public int IdContract { get; set; }

    [JsonPropertyName("is_pluralism_oter")] [Reactive] public bool IsPluralismOter { get; set; }
}