using System;
using System.IO;
using System.Text.Json;

namespace MyGame.Console.Core.SaveSystem
{
    public class SaveManager
    {
        private readonly string _savePath;

        public SaveManager()
        {
            _savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");
        }

        public void SaveGame(GameSaveData data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_savePath, json);
                System.Console.WriteLine("\n[Система] Игра успешно сохранена!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"\n[Ошибка] Не удалось сохранить игру: {ex.Message}");
            }
        }

        public GameSaveData? LoadGame()
        {
            try
            {
                if (!File.Exists(_savePath))
                {
                    System.Console.WriteLine("\n[Система] Файл сохранения не найден.");
                    return null;
                }

                string json = File.ReadAllText(_savePath);
                var data = JsonSerializer.Deserialize<GameSaveData>(json);
                System.Console.WriteLine("\n[Система] Игра успешно загружена!");
                return data;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"\n[Ошибка] Не удалось загрузить игру: {ex.Message}");
                return null;
            }
        }
    }
}