namespace NewContactBook
{
    

    public class ContactBookF
    {
        private List<Entry> entries;
        private bool changesMade = false;

        public ContactBookF()
        {
            entries = new List<Entry>();
            ShowWelcomeScreen();
            while (true)
            {
                MainMenu();
            }
        }

        public void ShowWelcomeScreen()
        {
            Console.Clear();
            string welcome = @"
Bienvenidos al libro de contactos!
Autor: Nathaniel Perez Marrero
Version: 4.0 
Last Revised: 2026-05-05 11;30 PM
Descripcion: This program allows you to keep records of your contacts.     ";


            Console.WriteLine(welcome);
            PressEnterToContinue();
        }

        public void MainMenu()
        {
            Console.Clear();
            string menu = @"
[1] Cargar contactos desde un archivo  
[2] Mostrar todos los contactos  
[3] Agregar contacto  
[4] Editar contacto  
[5] Eliminar contacto  
[6] Unir contactos duplicados  
[7] Guardar contactos en un archivo  
[8] Salir de la aplicación  
";
            Console.WriteLine(menu);
            int option = GetOption("seleccione una opcion: ", 1, 8);

            switch (option)
            {
                case 1: LoadEntriesFromFile(); break;
                case 2: ShowAllContacts(); break;
                case 3: AddContact(); break;
                case 4: EditContact(); break;
                case 5: DeleteContact(); break;
                case 6:
                    Console.Clear();
                    MergeDuplicates();
                    PressEnterToContinue();
                    break;
                case 7: SaveToFile(); break;
                case 8: ExitApplication(); break;
            }
        }

        public void LoadEntriesFromFile()
        {
            Console.Clear();
            Console.Write("Ingrese el nombre del archivo del que desea cargar o deje en blanco para cancelar: ");
            string filename = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Operacion Cancelada.");
                PressEnterToContinue();
                return;
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine($"ERROR: Archivo '{filename}' no fue encontrado.");
                PressEnterToContinue();
                return;
            }

            try
            {
                entries.Clear();
                using (StreamReader reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        Entry e = new Entry
                        {
                            FirstName = reader.ReadLine()?.Trim(),
                            LastName = reader.ReadLine()?.Trim(),
                            PhoneNumber = reader.ReadLine()?.Trim(),
                            Email = reader.ReadLine()?.Trim()
                        };
                        entries.Add(e);
                    }
                }
                Console.WriteLine("Contactos cargados con éxito.");
                changesMade = false;
            }
            catch
            {
                Console.WriteLine("ERROR: Ocurrió un error al leer el archivo.");
            }
            PressEnterToContinue();
        }

        public void ShowAllContacts()
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No hay contactos para mostrar.");
                PressEnterToContinue();
                return;
            }

            // Agregar menú de ordenación
            Console.Clear();
            Console.WriteLine("Selecciona el criterio de ordenación:");
            Console.WriteLine("[1] Nombre");
            Console.WriteLine("[2] Apellido");
            Console.WriteLine("[3] Teléfono");
            Console.WriteLine("[4] Correo electrónico");
            Console.Write("[M] Menú principal: ");

            string input = Console.ReadLine().Trim().ToUpper();

            switch (input)
            {
                case "1":
                    entries = entries.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList(); // Ordenar por Nombre, luego Apellido
                    break;
                case "2":
                    entries = entries.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList(); // Ordenar por Apellido, luego Nombre
                    break;
                case "3":
                    entries = entries.OrderBy(e => e.PhoneNumber).ToList(); // Ordenar por Teléfono
                    break;
                case "4":
                    entries = entries.OrderBy(e => e.Email).ToList(); // Ordenar por Correo electrónico
                    break;
                case "M":
                    return; // Regresar al menú principal
                default:
                    Console.WriteLine("Opción inválida.");
                    PressEnterToContinue();
                    return;
            }

            // Mostrar los contactos ordenados
            int pagina = 0;
            int ObPorPag = 10;
            int totalPag = (int)Math.Ceiling(entries.Count / (double)ObPorPag);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Índice Nombre             Apellido       Teléfono           Email");
                Console.WriteLine("---------------------------------------------------------------");
                for (int i = pagina * ObPorPag; i < Math.Min((pagina + 1) * ObPorPag, entries.Count); i++)
                {
                    Console.WriteLine($"{i + 1,5} {entries[i]}");
                }
                Console.WriteLine($"\nPágina {pagina + 1} de {totalPag}");

                Console.Write("[S]iguiente, [A]terior, [M]enú principal: ");
                input = Console.ReadLine().Trim().ToUpper();
                if (input == "S" && pagina < totalPag - 1) pagina++;
                else if (input == "A" && pagina > 0) pagina--;
                else if (input == "M") break;
            }
        }

        public void AddContact()
        {
            Console.Clear();
            Console.Write("Primer Nombre: ");
            string name = Console.ReadLine();
            Console.Write("Apellido: ");
            string lastName = Console.ReadLine();
            Console.Write("Numero de telefono: ");
            string phone = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("ERROR: El nombre y el apellido no pueden estar vacíos.");
                PressEnterToContinue();
                return;
            }

            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("ERROR: El teléfono y el correo electrónico no pueden estar vacíos.");
                PressEnterToContinue();
                return;
            }

            Console.Write("Confirmar agregar contacto? (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                entries.Add(new Entry { FirstName = name, LastName = lastName, PhoneNumber = phone, Email = email });
                Console.WriteLine("Contacto agregado exitosamente.");
                changesMade = true;
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
            PressEnterToContinue();
        }

        public void EditContact()
        {
            Console.Clear();
            Console.WriteLine("¿Cómo deseas buscar el contacto?");
            Console.WriteLine("[1] Nombre");
            Console.WriteLine("[2] Apellido");
            Console.WriteLine("[3] Teléfono");
            Console.WriteLine("[4] Correo electrónico");
            Console.WriteLine("[C] Cancelar");
            Console.Write("Opción: ");
            string option = Console.ReadLine().Trim().ToUpper();

            if (option == "C") return;

            string campo = option switch
            {
                "1" => "nombre",
                "2" => "apellido",
                "3" => "telefono",
                "4" => "email",
                _ => null
            };

            if (campo == null)
            {
                Console.WriteLine("Opción inválida.");
                PressEnterToContinue();
                return;
            }

            Console.Write($"Ingresa el valor del campo '{campo}' para buscar: ");
            string valor = Console.ReadLine().Trim().ToLower();

            var coincidencias = entries.Where(e =>
                (campo == "nombre" && e.FirstName?.ToLower().Contains(valor) == true) ||
                (campo == "apellido" && e.LastName?.ToLower().Contains(valor) == true) ||
                (campo == "telefono" && e.PhoneNumber?.ToLower().Contains(valor) == true) ||
                (campo == "email" && e.Email?.ToLower().Contains(valor) == true)
            ).ToList();

            if (coincidencias.Count == 0)
            {
                Console.WriteLine("No se encontraron coincidencias.");
                PressEnterToContinue();
                return;
            }

            Console.WriteLine("\nCoincidencias encontradas:");
            for (int i = 0; i < coincidencias.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {coincidencias[i]}");
            }

            Console.Write("\nSeleccione el número del contacto a editar o 'C' para cancelar: ");
            string seleccion = Console.ReadLine();
            if (seleccion.Trim().ToUpper() == "C") return;

            if (!int.TryParse(seleccion, out int index) || index < 1 || index > coincidencias.Count)
            {
                Console.WriteLine("Selección inválida.");
                PressEnterToContinue();
                return;
            }

            Entry e = coincidencias[index - 1];

            Console.WriteLine("\nEditando contacto:");
            Console.WriteLine(e);
            Console.Write("Nuevo nombre (deje en blanco para mantenerlo): ");
            string name = Console.ReadLine();
            Console.Write("Nuevo apellido (deje en blanco para mantenerlo): ");
            string lastName = Console.ReadLine();
            Console.Write("Nuevo número de teléfono (deje en blanco para mantenerlo): ");
            string phone = Console.ReadLine();
            Console.Write("Nuevo correo electrónico (deje en blanco para mantenerlo): ");
            string email = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) e.FirstName = name;
            if (!string.IsNullOrWhiteSpace(lastName)) e.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phone)) e.PhoneNumber = phone;
            if (!string.IsNullOrWhiteSpace(email)) e.Email = email;

            Console.WriteLine("Contacto actualizado.");
            changesMade = true;
            PressEnterToContinue();
        }

        public void DeleteContact()
        {
            Console.Clear();
            Console.WriteLine("¿Cómo deseas buscar el contacto para eliminar?");
            Console.WriteLine("[1] Nombre");
            Console.WriteLine("[2] Apellido");
            Console.WriteLine("[3] Teléfono");
            Console.WriteLine("[4] Correo electrónico");
            Console.WriteLine("[C] Cancelar");
            Console.Write("Opción: ");
            string option = Console.ReadLine().Trim().ToUpper();

            if (option == "C") return;

            string campo = option switch
            {
                "1" => "nombre",
                "2" => "apellido",
                "3" => "telefono",
                "4" => "email",
                _ => null
            };

            if (campo == null)
            {
                Console.WriteLine("Opción inválida.");
                PressEnterToContinue();
                return;
            }

            Console.Write($"Ingresa el valor del campo '{campo}' para buscar: ");
            string valor = Console.ReadLine().Trim().ToLower();

            var coincidencias = entries.Where(e =>
                (campo == "nombre" && e.FirstName?.ToLower().Contains(valor) == true) ||
                (campo == "apellido" && e.LastName?.ToLower().Contains(valor) == true) ||
                (campo == "telefono" && e.PhoneNumber?.ToLower().Contains(valor) == true) ||
                (campo == "email" && e.Email?.ToLower().Contains(valor) == true)
            ).ToList();

            if (coincidencias.Count == 0)
            {
                Console.WriteLine("No se encontraron coincidencias.");
                PressEnterToContinue();
                return;
            }

            Console.WriteLine("\nCoincidencias encontradas:");
            for (int i = 0; i < coincidencias.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {coincidencias[i]}");
            }

            Console.Write("\nSeleccione el número del contacto a eliminar o 'C' para cancelar: ");
            string seleccion = Console.ReadLine();
            if (seleccion.Trim().ToUpper() == "C") return;

            if (!int.TryParse(seleccion, out int index) || index < 1 || index > coincidencias.Count)
            {
                Console.WriteLine("Selección inválida.");
                PressEnterToContinue();
                return;
            }

            Entry e = coincidencias[index - 1];
            Console.WriteLine("\nEliminando contacto:");
            Console.WriteLine(e);
            Console.Write("Confirmar eliminación? (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                entries.Remove(e);
                Console.WriteLine("Contacto eliminado.");
                changesMade = true;
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }

            PressEnterToContinue();
        }

        public List<List<Entry>> FindDuplicatedContacts()
        {
            int n = entries.Count;

            int[] parent = new int[n];
            for (int i = 0; i < n; i++)
                parent[i] = i;

            int Find(int x)
            {
                if (parent[x] != x)
                    parent[x] = Find(parent[x]);
                return parent[x];
            }

            void Union(int a, int b)
            {
                int rootA = Find(a);
                int rootB = Find(b);
                if (rootA != rootB)
                    parent[rootB] = rootA;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var c1 = entries[i];
                    var c2 = entries[j];

                    bool sameName =
                        !string.IsNullOrWhiteSpace(c1.FirstName) &&
                        !string.IsNullOrWhiteSpace(c1.LastName) &&
                        string.Equals(c1.FirstName, c2.FirstName, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(c1.LastName, c2.LastName, StringComparison.OrdinalIgnoreCase);

                    bool samePhone =
                        !string.IsNullOrWhiteSpace(c1.PhoneNumber) &&
                        c1.PhoneNumber == c2.PhoneNumber;

                    bool sameEmail =
                        !string.IsNullOrWhiteSpace(c1.Email) &&
                        c1.Email.Equals(c2.Email, StringComparison.OrdinalIgnoreCase);

                    if (sameName || samePhone || sameEmail)
                        Union(i, j);
                }
            }

            var groups = new Dictionary<int, List<Entry>>();

            for (int i = 0; i < n; i++)
            {
                int root = Find(i);

                if (!groups.ContainsKey(root))
                    groups[root] = new List<Entry>();

                groups[root].Add(entries[i]);
            }

            return groups.Values.Where(g => g.Count > 1).ToList();
        }

        public void MergeDuplicates()
        {
            var duplicates = FindDuplicatedContacts();

            if (duplicates == null || duplicates.Count == 0)
            {
                Console.WriteLine("No se encontraron duplicados.");
                return;
            }

            foreach (var group in duplicates)
            {
                Console.WriteLine($"\nGrupo encontrado:");

                foreach (var e in group)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} | {e.PhoneNumber} | {e.Email}");
                }

                Console.Write("¿Deseas unir estos contactos? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() != "Y")
                    continue;

                // Crear contacto fusionado
                Entry merged = new Entry
                {
                    FirstName = group[0].FirstName,
                    LastName = group[0].LastName,
                    PhoneNumber = group.Select(x => x.PhoneNumber)
                                       .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),
                    Email = group.Select(x => x.Email)
                                 .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
                };

                Console.WriteLine("\nNuevo contacto:");
                Console.WriteLine($"{merged.FirstName} {merged.LastName} | {merged.PhoneNumber} | {merged.Email}");

                Console.Write("¿Agregar contacto fusionado? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    entries.Add(merged);
                }

                // Eliminar duplicados originales
                foreach (var e in group)
                {
                    entries.Remove(e);
                }

                changesMade = true;
                Console.WriteLine("Duplicados unidos correctamente.");
            }
        }

        public void SaveToFile()
        {
            Console.Clear();
            Console.Write("Ingrese el nombre del archivo para guardar o deje en blanco para cancelar: ");
            string filename = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Operación cancelada.");
                PressEnterToContinue();
                return;
            }

            if (File.Exists(filename))
            {
                Console.Write("El archivo existe. ¿Deseas sobrescribirlo? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() != "Y")
                {
                    Console.WriteLine("Operación cancelada.");
                    PressEnterToContinue();
                    return;
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (Entry e in entries)
                    {
                        writer.WriteLine(e.FirstName);
                        writer.WriteLine(e.LastName);
                        writer.WriteLine(e.PhoneNumber);
                        writer.WriteLine(e.Email);
                    }
                }
                Console.WriteLine("Contactos guardados exitosamente.");
                changesMade = false;
            }
            catch
            {
                Console.WriteLine("ERROR: No se pudo escribir en el archivo.");
            }
            PressEnterToContinue();
        }

        public void ExitApplication()
        {
            if (changesMade)
            {
                Console.Write("Se han detectado cambios. ¿Salir sin guardar? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() != "Y") return;
            }
            Console.WriteLine("¡Gracias por usar Contact Book. ¡Adiós!");
            Environment.Exit(0);
        }

        private int GetOption(string prompt, int min, int max)
        {
            int option;
            Console.Write(prompt);
            string input = Console.ReadLine();
            while (!int.TryParse(input, out option) || option < min || option > max)
            {
                Console.WriteLine("Opción inválida.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return option;
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("\nPresione ENTER para continuar...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        /*private void ShowAllContactsSimple()
        {
            Console.WriteLine("Índice Nombre             Apellido       Teléfono           Email");
            Console.WriteLine("---------------------------------------------------------------");
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"{i + 1,5} {entries[i]}");
            }
        } */
    }

   
}

