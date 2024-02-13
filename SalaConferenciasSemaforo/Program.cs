using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SalaConferenciasSemaforo
{

    public class Microfono
    {
        public SemaphoreSlim semaphore = new SemaphoreSlim(1); // Un semáforo para controlar el acceso al micrófono

        public void Hablar(string nombreAsistente)
        {
            Console.WriteLine($"{nombreAsistente} está hablando por el micrófono.");
            Thread.Sleep(4000); // Simula la duración del discurso del asistente
            Console.WriteLine($"{nombreAsistente} ha terminado de hablar.");
            Thread.Sleep(2000);
        }

        public async void SolicitarTurno(string nombreAsistente)
        {
            await semaphore.WaitAsync(); // Espera hasta que el semáforo esté disponible
            Hablar(nombreAsistente);
            semaphore.Release(); // Libera el semáforo después de hablar
        }
    }

    public class SalaConferencias
    {
        private Microfono[] microfonos;

        public SalaConferencias(int numMicrofonos)
        {
            microfonos = new Microfono[numMicrofonos];
            for (int i = 0; i < numMicrofonos; i++)
            {
                microfonos[i] = new Microfono();
            }
        }

        public void AsistenteQuiereHablar(string nombreAsistente)
        {
            bool micDisponible = false;
            foreach (Microfono microfono in microfonos)
            {
                if (micDisponible == false && microfono.semaphore.CurrentCount > 0)
                {
                    micDisponible = true;
                    microfono.SolicitarTurno(nombreAsistente);
                }
            }
            if (!micDisponible)
            {
                Console.WriteLine($"{nombreAsistente} no pudo hablar, todos los micrófonos están ocupados.");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            SalaConferencias sala = new SalaConferencias(0); // Crear una sala de conferencias con 3 micrófonos

            // Crear algunos asistentes que quieren hablar
            for (int i = 1; i <= 5; i++)
            {
                string nombreAsistente = "Asistente " + i;
                sala.AsistenteQuiereHablar(nombreAsistente);
            }

            Console.ReadLine();
        }
    }

}
