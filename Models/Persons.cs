using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaDesktop.Models;

public sealed class Persons : ReactiveObject
{
    #region Свойства персоны

    private readonly string _urlStorage = ConfigurationManager.AppSettings["host"] 
                                          ?? throw new NullReferenceException("Uninitialized property: " + nameof(_urlStorage));
    public string FullName => $"{FirstName} {MidlleName} {LastName}";
    [JsonPropertyName("id")] [Reactive] public int Id { get; set; }
    [JsonPropertyName("id_pers_pos")] [Reactive] public int IdPersPos { get; set; }
    [JsonPropertyName("description")] [Reactive] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("firstname")] [Reactive] public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("name")] [Reactive] public string MidlleName { get; set; } = string.Empty;
    [JsonPropertyName("lastname")] [Reactive] public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("gender")] [Reactive] public string Gender { get; set; } = string.Empty;
    [JsonPropertyName("phone_ua")] [Reactive] public string PhoneUkraine { get; set; } = string.Empty;
    [JsonPropertyName("phone_lug")] [Reactive] public string PhoneLugakom { get; set; } = string.Empty;
    public string ShortDate => Birthday.ToShortDateString();
    [JsonPropertyName("birthday")] [Reactive] public DateTime Birthday { get; set; }
    [JsonPropertyName("adress")] [Reactive] public string Adress { get; set; } = string.Empty;
    [JsonPropertyName("adress_rus")] [Reactive] public string AdressRus { get; set; } = string.Empty;
    [JsonPropertyName("data_start_contract")] [Reactive] public DateTime? StartDateContract { get; set; } 
    [JsonPropertyName("data_end_contract")] [Reactive] public DateTime? EndDateContract { get; set; }

    [JsonPropertyName("id_dep")] [Reactive] public int IdDepartment { get; set; }

    [JsonPropertyName("name_depart")] [Reactive] public string DepartmentName { get; set; } = string.Empty;

    #region Место рождения 
    // город
    [JsonPropertyName("city_of_birth")]
    public string CityOfBirth { get; set; } = string.Empty;
    // Регион
    [JsonPropertyName("region_of_birth")]
    public string RegionOfBirth { get; set; } = string.Empty;
    // Область
    [JsonPropertyName("area_of_birth")]
    public string AreaOfBirth { get; set; } = string.Empty;
    // Страна
    [JsonPropertyName("country_of_birth")]
    public string CountryOfBirth { get; set; } = string.Empty;

    [JsonPropertyName("subdivision_code")]
    public string SubdivisionCode { get; set; } = string.Empty;

    #endregion

    private string? _photo;
    [JsonPropertyName("photo")]
    public string Photo
    {
        get => _photo == "static-images/images/no-photo.png" ? $"{_urlStorage}{_photo}" : $"{_urlStorage}storage{_photo}";

        set => _photo = value;
    }

    [JsonPropertyName("number_snils")]
    public string SnilsNumber { get; set; } = string.Empty;

    [JsonPropertyName("inn_passport_rus")]
    public string IdentificationCodeRus { get; set; } = string.Empty;


    [JsonPropertyName("searial_passport")]
    public string SerialPassport { get; set; } = string.Empty;

    [JsonPropertyName("number_passport")]
        public string NumberPassport { get; set; } = string.Empty;

    [JsonPropertyName("organization_passport")]
        public string OrganizationPassport { get; set; } = string.Empty;

    [JsonPropertyName("date_passport")]
        public DateTime? DatePassport { get; set; }


    [JsonPropertyName("searial_passport_rus")]
    public string SerialPassportRus { get; set; } = string.Empty;

    [JsonPropertyName("number_passport_rus")]
    public string NumberPassportRus { get; set; } = string.Empty;

    [JsonPropertyName("organization_passport_rus")]
    public string OrganizationPassportRus { get; set; } = string.Empty;

    [JsonPropertyName("date_passport_rus")]
    public DateTime? DatePassportRus { get; set; }

    [JsonPropertyName("type_passport")]
        public string TypePassport { get; set; } = string.Empty;

    [JsonPropertyName("position_pluralist")]
    public string PositionPluralist { get; set; } = string.Empty;

    [JsonPropertyName("skud_card")]
    public string CodeCard { get; set; } = string.Empty;

    #endregion

    #region Bool 

    [JsonPropertyName("is_pensioner")]
    public bool IsPensioner { get; set; }

    [JsonPropertyName("is_ped")]
    public bool IsPed { get; set; }

    [JsonPropertyName("is_student")]
    public bool IsStudent { get; set; }

    [JsonPropertyName("is_graduate")]
    public bool IsGraduate { get; set; }

    [JsonPropertyName("is_doctor")]
    public bool IsDoctor { get; set; }

    
    [JsonPropertyName("is_pluralism_inner")]
    public bool IsPluralismInner { get; set; }
    
    [JsonPropertyName("is_pluralism_oter")]
    public bool IsPluralismOter { get; set; }

    [JsonPropertyName("is_single_mother")]
    public bool IssingleMother { get; set; }
    [JsonPropertyName("is_two_child_mother")]
    public bool IsTwoChildMother { get; set; }
    [JsonPropertyName("is_previos_convition")]
    public bool IsPreviosConvition { get; set; }

    [JsonPropertyName("is_responsible")]
    public bool IsResponsible { get; set; }

    [JsonPropertyName("is_main")]
    public bool IsMain { get; set; }

    [JsonPropertyName("is_rus")]
    public bool IsRussion { get; set; }

    [JsonPropertyName("is_snils")]
    public bool IsSnils { get; set; }

    #endregion

    #region Массивы

    [JsonPropertyName("positionsOfDepartment")] [Reactive] public ObservableCollection<Position> ArrayPosition { get; set; } = new();

    #endregion

    [JsonPropertyName("stavka_budget")] [Reactive] public decimal StavkaBudget { get; set; }

    [JsonPropertyName("stavka_nobudget")] [Reactive] public decimal StavkaNoBudget { get; set; }

    [JsonPropertyName("identification_code")] [Reactive] public string IdentificationCode { get; set; } = string.Empty;

    [JsonPropertyName("full_age")] public FullAge? FullAge { get; set; }

    [JsonPropertyName("date_working")] [Reactive] public DateTime? DateWorking { get; set; }

    [JsonPropertyName("date_order")] [Reactive] public DateTime? DateOrder { get; set; }

    [JsonPropertyName("order")] [Reactive] public string OrderName { get; set; } = string.Empty;

    [JsonPropertyName("date_insert")] [Reactive] public DateTime? OrderDate { get; set; }
    [JsonPropertyName("position")] [Reactive] public string PersonPosition { get; set; } = string.Empty;
}