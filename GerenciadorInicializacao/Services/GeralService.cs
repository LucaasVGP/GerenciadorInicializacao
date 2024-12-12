using GerenciadorInicializacao.Enums;
using GerenciadorInicializacao.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorInicializacao.Services
{
    public static class GeralService
    {
        public static void TocarAudio()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // Concatena com o caminho relativo onde está o som
            string audioPath = Path.Combine(basePath, "som.mp3");

            if (!File.Exists(audioPath))
            {
                Console.WriteLine("Arquivo de áudio não encontrado: " + audioPath);
                return;
            }

            using (var audioFile = new AudioFileReader(audioPath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Volume = 0.1f;

                outputDevice.Init(audioFile);
                outputDevice.Play();
                Console.WriteLine("Finalizado...");
                Console.ReadLine();
            }
        }

        public static void AbrirProgramas(List<Aplicativo> apps)
        {
            var dataAtual = DateTime.Now;
            foreach (var app in apps)
            {
                try
                {
                    switch (app.TipoAbertura)
                    {
                        case ETipoAbertura.FinalSemana:
                            if (dataAtual.DayOfWeek != DayOfWeek.Saturday && dataAtual.DayOfWeek != DayOfWeek.Sunday) continue;
                            break;

                        case ETipoAbertura.Semana:
                            if (dataAtual.DayOfWeek == DayOfWeek.Saturday || dataAtual.DayOfWeek == DayOfWeek.Sunday) continue;
                            break;
                    }

                    if (!EstaAberto(app.NomeProcesso))
                    {
                        var program = Process.Start(new ProcessStartInfo
                        {
                            FileName = app.Caminho,
                            Arguments = app.Argumento ?? "",
                            UseShellExecute = true
                        });
                        System.Threading.Thread.Sleep(5000);
                        var abriu = program.MainWindowHandle != IntPtr.Zero;
                        var contador = 1;
                        while (!abriu)
                        {
                            Console.WriteLine($"iniciando... {app.Nome}");
                            System.Threading.Thread.Sleep(1000);
                            if (contador > 5) abriu = true;
                            contador++;
                        }
                        Console.WriteLine($"Programa iniciado: {app.Nome}");
                        continue;
                    }

                    Console.WriteLine($"Programa ja iniciado: {app.Nome}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao iniciar {app.Nome}: {ex.Message}");
                }
            }
        }

        private static bool EstaAberto(string nomeDoProcesso)
        {
            Process[] processos = Process.GetProcessesByName(nomeDoProcesso);
            return processos.Length > 0;
        }
    }
}