using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace STUDENTS_LAB_TRY_2
{
    public partial class Form1 : Form
    {

        List<Student> students = new List<Student>
        {
        };
        List<Student> filteredStudents;
        int currentIndex = 0;

        public Form1()
        {

            InitializeComponent();
            ClearInput();
            UpdateCurrentStudent();
        }

        private void FilterStudents()
        {
            string selectedAttribute = comboBox1.SelectedItem.ToString(); // выбранный атрибут из комбобокса
            string filterValue = textBox4.Text;

            filteredStudents = students.Where(s => s.Name.ToLower().Contains(filterValue.ToLower())).ToList();

            if (filteredStudents.Count > 0)
            {
                // если список не пуст, вывести информацию о первом студенте
                currentIndex = 0;
                DisplayStudent(filteredStudents[currentIndex]);
                button1.Enabled = false;
                button2.Enabled = filteredStudents.Count > 1;
            }

            else
            {
                // если список пуст, выключить кнопки "Предыдущий" и "Следующий"
                DisplayStudent(null);
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void ClearInput()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void DisplayStudent()
        {
            textBox1.Text = students[currentIndex].Name;
            textBox2.Text = students[currentIndex].Surname;
            textBox3.Text = students[currentIndex].Faculty;

            if (students[currentIndex] is Master)
            {
                var master = (Master)students[currentIndex];
                textBox5.Text = master.BachelorDiplomaNumber;
                textBox6.Text = master.BachelorGraduationDate;
            }

        }

        private void DisplayStudent(Student student)
        {
            textBox1.Text = student.Name;
            textBox2.Text = student.Surname;
            textBox3.Text = student.Faculty;
            if (student is Master)
            {
                var master = (Master)student;
                textBox5.Text = master.BachelorDiplomaNumber;
                textBox6.Text = master.BachelorGraduationDate;
            }
        }


        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearInput();
            students.Clear();
            students.Add(new Student());
            currentIndex = 0;
            UpdateCurrentStudent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Открытие существующего списка студентов
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|JSON files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                string fileExtension = Path.GetExtension(fileName);
                try
                {
                    if (fileExtension == ".xml")
                    {
                        // получаем путь к выбранному файлу
                        var path = openFileDialog.FileName;

                        // создаем объект XmlSerializer для десериализации из XML
                        var serializer = new XmlSerializer(typeof(List<Student>), new[] { typeof(Master) });

                        // открываем файл для чтения сериализованного объекта
                        var reader = new StreamReader(path);

                        // десериализуем список объектов из XML и выводим его на экран
                        students = (List<Student>)serializer.Deserialize(reader);
                        currentIndex = 0;
                        DisplayStudent();

                    }
                    else if (fileExtension == ".json")
                    {
                        string json = File.ReadAllText("students.json");

                        // Десериализуем JSON строку в объект Student[]
                        students = JsonConvert.DeserializeObject<List<Student>>(json);

                        // Выводим данные первого студента в текстбоксы
                        DisplayStudent(students[0]);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            UpdateCurrentStudent();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (students.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    try
                    {
                        if (fileExtension == ".xml")
                        {
                            var serializer = new XmlSerializer(typeof(List<Student>), new[] { typeof(Master) });

                            var path = saveFileDialog.FileName;
                            var file = new StreamWriter(path);

                            // сериализуем список объектов в XML и записываем его в файл
                            serializer.Serialize(file, students);

                            // закрываем файл
                            file.Close();
                        }
                        else if (fileExtension == ".json")
                        {
                            // Сериализация списка студентов в JSON-файл
                            string json = JsonConvert.SerializeObject(students);
                            File.WriteAllText(fileName, json);
                        }
                        MessageBox.Show("Список студентов сохранен в файл.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Список студентов пуст. Нечего сохранять.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            students.Add(new Student());
            currentIndex++;
            ClearInput();
            UpdateCurrentStudent();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            students[currentIndex].Name = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            students[currentIndex].Surname = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            students[currentIndex].Faculty = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0) // Проверяем, что не находимся в начале списка
            {
                currentIndex--; // Уменьшаем индекс текущего студента
                DisplayStudent(); // Обновляем отображение текущего студента
                UpdateCurrentStudent(); // Обновляем состояние кнопок навигации
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentIndex < students.Count - 1) // Проверяем, что не находимся в конце списка
            {
                currentIndex++; // Увеличиваем индекс текущего студента
                DisplayStudent(); // Обновляем отображение текущего студента
                UpdateCurrentStudent(); // Обновляем состояние кнопок навигации
            }
        }

        private void предыдущийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0) // Проверяем, что не находимся в начале списка
            {
                currentIndex--; // Уменьшаем индекс текущего студента
                DisplayStudent(); // Обновляем отображение текущего студента
                UpdateCurrentStudent(); // Обновляем состояние кнопок навигации
            }
        }

        private void следующийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentIndex < students.Count - 1) // Проверяем, что не находимся в конце списка
            {
                currentIndex++; // Увеличиваем индекс текущего студента
                DisplayStudent(); // Обновляем отображение текущего студента
                UpdateCurrentStudent(); // Обновляем состояние кнопок навигации
            }
        }

        private void UpdateCurrentStudent()
        {
            if (students.Count > 0 && students[currentIndex] is Master)
            {
                label6.Visible = true;
                label7.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                button3.Visible = false;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                button3.Visible = true;
            }
            // обновляем поля на форме с информацией о текущем студенте
            if (students.Any())
            {
                DisplayStudent();
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;

            }

            // отключаем поля на форме, если список студентов пуст
            textBox1.Enabled = students.Any();
            textBox2.Enabled = students.Any();
            textBox3.Enabled = students.Any();
            textBox4.Enabled = students.Any();
            textBox5.Enabled = students.Any();
            textBox6.Enabled = students.Any();
            добавитьToolStripMenuItem.Enabled = students.Any() || currentIndex == -1;
            удалитьToolStripMenuItem.Enabled = students.Any();
            сохранитьToolStripMenuItem.Enabled = students.Any();
            comboBox1.Enabled = students.Any();
            button1.Enabled = students.Any() & currentIndex > 0; // Устанавливаем состояние кнопки "Предыдущий" в зависимости от нахождения в начале списка
            button2.Enabled = students.Any() & currentIndex < students.Count - 1; // Устанавливаем состояние кнопки "Следующий" в зависимости от нахождения в конце списка
            button3.Enabled = students.Any();
            предыдущийToolStripMenuItem.Enabled = button1.Enabled; // Обновляем состояние пункта меню "Просмотр -> Предыдущий"
            следующийToolStripMenuItem.Enabled = button2.Enabled; // Обновляем состояние пункта меню "Просмотр -> Следующий"
            удалитьToolStripMenuItem.Enabled = students.Any();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearInput();
            students.RemoveAt(currentIndex);

            // если список студентов пуст, переходим к первому студенту
            if (!students.Any())
            {
                currentIndex = -1;
            }
            else
            {
                // если был удален последний студент, переходим к предыдущему студенту
                if (currentIndex >= students.Count)
                {
                    currentIndex = students.Count - 1;
                }
            }

            UpdateCurrentStudent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }


        private void SearchStudent(string field, string value, List<Student> lst)
        {


            switch (field)
            {
                case "Имя":
                    for (int i = 0; i < lst.Count; i++)
                    {
                        students[i] = lst[i];
                    }
                    students.Clear();
                    foreach (Student student in students)
                    {
                        if (student.Name == value)
                        {
                            students.Add(student);
                        }
                    }
                    if (students.Count > 0)
                    {
                        DisplayStudent(students[0]);
                    }
                    else
                    {
                       ClearInput();
                    }
                    break;
                case "Фамилия":
                    students.Clear();
                    foreach (Student student in lst)
                    {
                        if (student.Surname == value)
                        {
                            students.Add(student);
                        }
                    }
                    if (students.Count > 0)
                    {
                        DisplayStudent(students[0]);
                    }
                    else
                    {
                        ClearInput();
                    }
                    break;
                case "Факультет":
                    students.Clear();
                    foreach (Student student in lst)
                    {
                        if (student.Faculty == value)
                        {
                            students.Add(student);
                        }
                    }
                    if (students.Count > 0)
                    {
                        DisplayStudent(students[0]);
                    }
                    else
                    {
                        ClearInput();
                    }
                    break;
                default:
                    MessageBox.Show("Invalid field name!");
                    return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            var master = (Master)students[currentIndex];
            master.BachelorDiplomaNumber = textBox5.Text;

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            var master = (Master)students[currentIndex];
            master.BachelorGraduationDate = textBox6.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var master = new Master
            {
                Name = students[currentIndex].Name,
                Surname = students[currentIndex].Surname,
                Faculty = students[currentIndex].Faculty,
                BachelorDiplomaNumber = "",
                BachelorGraduationDate = ""
            };
            students.Remove(students[currentIndex]);
            students.Add(master);
            students[currentIndex] = master;
            UpdateCurrentStudent();
            DisplayStudent();
        }

    }





    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Faculty { get; set; }

        public Student() { }

        public Student(string name, string surname, string faculty)
        {
            this.Name = name;
            this.Surname = surname;
            this.Faculty = faculty;
        }


    }

    [XmlInclude(typeof(Master))]
    public class Master : Student
    {
        public string BachelorDiplomaNumber { get; set; }
        public string BachelorGraduationDate { get; set; }

        public Master() { }

        public Master(string name, string surname, string faculty, string bdn, string bgd)
        {
            Name = name;
            Surname = surname;
            Faculty = faculty;
            BachelorDiplomaNumber = bdn;
            BachelorGraduationDate = bgd;
        }
    }


}
