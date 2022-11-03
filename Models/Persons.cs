using System;
using System.Configuration;
using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models;

public sealed class Persons
{
    #region Свойства персоны

    private readonly string _urlStorage = ConfigurationManager.AppSettings["host"] 
                                          ?? throw new NullReferenceException("Uninitialized property: " + nameof(_urlStorage));
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("id_pers_pos")]
    public int IdPersPos { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string MidlleName { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("phone_ua")]
    public string PhoneUkraine { get; set; } = string.Empty;

    [JsonPropertyName("phone_lug")]
    public string PhoneLugakom { get; set; } = string.Empty;

    public string ShortDate => Birthday.ToShortDateString();

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }

    [JsonPropertyName("adress")]
    public string Adress { get; set; } = string.Empty;

    [JsonPropertyName("adress_rus")]
    public string AdressRus { get; set; } = string.Empty;

    [JsonPropertyName("data_start_contract")]
    public DateTime? StartDateContract { get; set; } 
    [JsonPropertyName("data_end_contract")]
    public DateTime? EndDateContract { get; set; }

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

    [JsonPropertyName("stavka_budget")]
    public decimal StavkaBudget { get; set; }

    [JsonPropertyName("stavka_nobudget")]
    public decimal StavkaNoBudget { get; set; }

    [JsonPropertyName("identification_code")]
    public string IdentificationCode { get; set; } = string.Empty;

    [JsonPropertyName("full_age")]
    public FullAge? FullAge { get; set; }

    [JsonPropertyName("date_working")]
    public DateTime? DateWorking { get; set; }

    [JsonPropertyName("date_order")] public DateTime? DateOrder { get; set; }

    [JsonPropertyName("order")] public string OrderName { get; set; } = string.Empty;

    [JsonPropertyName("date_insert")] public DateTime? OrderDate { get; set; }
    [JsonPropertyName("position")] public string PersonPosition { get; set; } = string.Empty;
}