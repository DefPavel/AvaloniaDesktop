using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace AvaloniaDesktop.Helpers;

public static class CustomAes256
{
    #region Конвертация из Byte в Hex String

    private static string ByteArrayToString(IReadOnlyCollection<byte> ba)
    {
        StringBuilder hex = new(ba.Count * 2);
        foreach (var b in ba)
        {
            _ = hex.Append($"{b:x2}");
        }

        return hex.ToString();
    }

    #endregion

    #region Кодирование из String в Hex

    private static byte[] StringToByteArray(string hex)
    {
        var numberChars = hex.Length;
        var bytes = new byte[numberChars / 2];
        for (var i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return bytes;
    }
    #endregion

    #region  Кодирование данных Формата IV : Данные
    public static string Encrypt(string plainText, string key)
    {
        // Проверяем аргуметы 
        if (plainText is null)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        byte[] encrypted;
        byte[] iv;
        // Создайте объект RijndaelManaged
        // с указанным ключом и IV. 
        //using var AES = Aes.Create("AesManaged");
#pragma warning disable SYSLIB0022 // Тип или член устарел
        using (RijndaelManaged rijAlg = new())
#pragma warning restore SYSLIB0022 // Тип или член устарел
        {
            //Записываем ключ в байтах
            rijAlg.Key = Encoding.UTF8.GetBytes(key);
            //Генерируем массив вектора в байтах
            rijAlg.GenerateIV();
            iv = rijAlg.IV;

            // TransformFinalBlock - это специальная функция для преобразования последнего блока или частичного блока в потоке.
            // Возвращает новый массив, содержащий оставшиеся преобразованные байты. Возвращается новый массив, потому что количество
            // информация, возвращаемая в конце, может быть больше одного блока при добавлении заполнения.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Создаём поток для записи шифрования. 
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                // Записываем все данные в поток
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }
        // Возвращаем данные в строке
        return ByteArrayToString(iv) + ":" + ByteArrayToString(encrypted);

    }
    #endregion

    #region Декодирование данных формата IV : Данные
    static string Decrypt(string cipherText, string key)
    {
        // Проверяем аргуметы 
        if (cipherText is not { Length: > 0 })
        {
            throw new ArgumentNullException("Текст декодирования является пустым");
        }

        if (key is not { Length: > 0 })
        {
            throw new ArgumentNullException("Ключ декодирования является пустым");
        }

        //Разбиваем значение до ":"
        var splitchiper = cipherText.Split(':');

        //Записываем второе значение
        var chiperByte = StringToByteArray(splitchiper[1]);

        // Создайте объект RijndaelManaged
        // с указанным ключом и IV. 
#pragma warning disable SYSLIB0022 // Тип или член устарел
        using RijndaelManaged rijAlg = new();
#pragma warning restore SYSLIB0022 // Тип или член устарел
        rijAlg.Key = Encoding.UTF8.GetBytes(key);
        rijAlg.GenerateIV();

        // Создаём дешифратор для выполнения преобразования потока.
        var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        // Создаём поток для чтения дешифрованных данных (байтов). 
        using MemoryStream msDecrypt = new(chiperByte);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);
        // Объявляем строку, используемую для хранения
        // расшифрованный текст.
        // Читаем дешифрованные байты из потока дешифрования
        // и помещаем их в строку.
        string plaintext = srDecrypt.ReadToEnd();
        return plaintext;
    }
    #endregion

}

