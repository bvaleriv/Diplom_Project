using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace KursRabota
{
    public partial class Redactor : Form
    {
        // вспомогательные переменные - в них будут хранится обработанные значения
        double a = 0, b = 0, c = -15; // выбранные оси        
        bool Wire = false;
        double outputKoefX, outputKoefY;

        public Redactor()
        {
            InitializeComponent();
            AnT.InitializeContexts();
            outputKoefX = AnT.Width / 16.6;
            outputKoefY = AnT.Height / 12.4;
            
        }

        private void Redactor_Load(object sender, EventArgs e)
        {
            // инициализация бибилиотеки glut 
            Glut.glutInit();
            // инициализация режима экрана 
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // установка цвета очистки экрана (RGBA) 
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода 
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();

            // установка перспективы 
            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // начальная настройка параметров openGL (тест глубины, освещение и первый источник света) 
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            
            label2.Visible = label3.Visible =false;
            textBox1.Visible = textBox2.Visible = false;
        }

        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите элемент из списка");
            }
            else
            {
                a = e.X;
                b = e.Y;
                
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Нужно ввести данные!!!", "Предупреждение");
                }
                else
                {
                    Draw();
                    if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 2)
                        write(Convert.ToDouble(a / outputKoefX * 2), Convert.ToDouble(-b / outputKoefY * 2), Convert.ToDouble(textBox1.Text));
                    else
                        write(Convert.ToDouble(a / outputKoefX * 2), Convert.ToDouble(-b / outputKoefY * 2), Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
                }
            }
        }
        
        public void seting_panel()
        {
            // очистка буфера цвета и буфера глубины 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            // очищение текущей матрицы 
            Gl.glLoadIdentity();
            Gl.glTranslated(-16.6, 12.4, c); //перемещаем центр координат в центр
            Gl.glClearColor(255,255,255,1);
            // помещаем состояние матрицы в стек матриц , дальнейшие трансформации затронут только визуализацию объекта 
            Gl.glPushMatrix();
            // производим перемещение, в зависимости от значений
            Gl.glTranslated(a / outputKoefX * 2, -b / outputKoefY * 2, c);
            // содержимое буфера накопления выводится в буфер кадра
            Gl.glAccum(Gl.GL_RETURN, 1f);
        }

        private void Draw()
        {
            //в зависимсоти от установленного типа объекта 
            switch (comboBox1.SelectedIndex)
            {
                // рисуем нужный объект, использую фунции бибилиотеки GLUT 
                case 0:
                    {
                        if (Wire)
                        {
                            seting_panel();
                            double r = Convert.ToDouble(textBox1.Text);
                            Glut.glutWireSphere(r, 50, 50);

                        }
                        else
                        {
                            seting_panel();
                            double r = Convert.ToDouble(textBox1.Text);
                            Glut.glutSolidSphere(r, 50, 50); // полигональная сфера 
                        }

                        break;
                    }
                case 1:
                    {
                        if (Wire)
                        {
                            seting_panel();
                            double r = Convert.ToDouble(textBox1.Text);
                            double h = Convert.ToDouble(textBox2.Text);
                            Glut.glutWireCylinder(r, h, 60, 60);
                        }
                        else
                        {
                            seting_panel();
                            double r = Convert.ToDouble(textBox1.Text);
                            double h = Convert.ToDouble(textBox2.Text);
                            Glut.glutSolidCylinder(r, h, 60, 60);
                        }
                        break;
                    }
                case 2:
                    {
                        if (Wire)
                        {
                            seting_panel();
                            double s = Convert.ToDouble(textBox1.Text);
                            Glut.glutWireCube(s);
                        }
                        else
                        {
                            seting_panel();
                            double s = Convert.ToDouble(textBox1.Text);
                            Glut.glutSolidCube(s);
                        }
                        break;
                    }
                case 3:
                    {
                        if (Wire)
                        {
                            seting_panel();
                            Gl.glRotated(270, 20, 0, 0);
                            double r = Convert.ToDouble(textBox1.Text);
                            double h = Convert.ToDouble(textBox2.Text);
                            Glut.glutWireCone(r, h, 60, 60);
                        }
                        else
                        {
                            seting_panel();
                            Gl.glRotated(270, 20, 0, 0);
                            double r = Convert.ToDouble(textBox1.Text);
                            double h = Convert.ToDouble(textBox2.Text);
                            Glut.glutSolidCone(r, h, 60, 60);
                        }
                        break;
                    }
                case 4:
                    {
                        if (Wire)
                        {
                            seting_panel();
                            double R = Convert.ToDouble(textBox1.Text);
                            double r = Convert.ToDouble(textBox2.Text);
                            Glut.glutWireTorus(r, R, 60, 60);
                        }
                        else
                        {
                            seting_panel();
                            double R = Convert.ToInt32(textBox1.Text);
                            double r = Convert.ToInt32(textBox2.Text);
                            Glut.glutSolidTorus(r, R, 60, 60);
                        }
                        break;
                    }
            }

            // возвращаем состояние матрицы 
            Gl.glPopMatrix();
            // завершаем рисование 
            Gl.glFlush();
            Gl.glAccum(Gl.GL_LOAD, 1f);
            // обновлем элемент AnT 
            AnT.Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                // устанавливаем сеточный режим визуализации 
                Wire = true;
            }
            else
            {
                // иначе - полигональная визуализация 
                Wire = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Location = new Point(855, textBox1.Location.Y);
                label2.Text = "Введите радиус:";
                label2.Visible = textBox1.Visible = true;
                label3.Visible = textBox2.Visible = false;
                textBox1.Location = new Point(textBox1.Location.X - 75, textBox1.Location.Y);
                checkBox1.Visible=checkBox2.Visible = true;
                //label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Location = new Point(855, textBox1.Location.Y);
                textBox2.Location = new Point(855, textBox2.Location.Y);
                label2.Text = "Введите радиус:";
                label3.Text = "Введите высоту:";
                label2.Visible = label3.Visible = textBox1.Visible = textBox2.Visible = true;
                textBox1.Location = new Point(textBox1.Location.X - 75, textBox1.Location.Y);
                textBox2.Location = new Point(textBox2.Location.X - 75, textBox2.Location.Y);
                checkBox1.Visible = checkBox2.Visible = true;
                //label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Location = new Point(855, textBox1.Location.Y);
                label2.Text = "Введите высоту:";
                label2.Visible = textBox1.Visible = true;
                label3.Visible = textBox2.Visible = false;
                textBox1.Location = new Point(textBox1.Location.X - 75, textBox1.Location.Y);
                checkBox1.Visible = checkBox2.Visible = true;
                //label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
            if (comboBox1.SelectedIndex == 3)
            {
                textBox1.Location = new Point(855, textBox1.Location.Y);
                textBox2.Location = new Point(855, textBox2.Location.Y);
                label2.Text = "Введите радиус:";
                label3.Text = "Введите высоту:";
                label2.Visible = label3.Visible = textBox1.Visible = textBox2.Visible = true;
                textBox1.Location = new Point(textBox1.Location.X - 75, textBox1.Location.Y);
                textBox2.Location = new Point(textBox2.Location.X - 75, textBox2.Location.Y);
                checkBox1.Visible = checkBox2.Visible = true;
                //label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
            if (comboBox1.SelectedIndex == 4)
            {
                textBox1.Location = new Point(855, textBox1.Location.Y);
                textBox2.Location = new Point(855, textBox2.Location.Y);
                label2.Text = "Введите наружный диаметр:";
                label3.Text = "Введите внутренний диаметр:";
                label2.Visible = label3.Visible = textBox1.Visible = textBox2.Visible = true;
                checkBox1.Visible = checkBox2.Visible = true;
                //label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите элемент из списка");
            }
            else
            {
                if (textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Нужно заполнить данные");
                }
                else
                {
                    button3.Visible = true;
                    button3.Enabled = true;
                    a = Convert.ToInt32(textBox3.Text);
                    b = Convert.ToInt32(textBox4.Text);
                    Draw();
                }
            }
        }

        //запись данных в файл с 3 параметрами
        public void write(double x, double y, double r)
        {
            //в зависимсоти от установленного типа объекта 
            switch (comboBox1.SelectedIndex)
            {
                // записываем исходные данные выбранного объекта
                case 0:
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Нужно ввести радиус сферы", "Предупреждение");
                        }
                        else
                        {
                            StreamWriter sphera = new StreamWriter("Сфера.txt");
                            sphera.WriteLine("Исходные данные сферы");
                            sphera.WriteLine("Координаты Х|Координаты Y|Радиус");
                            sphera.WriteLine("-------------------------------");
                            sphera.WriteLine(x + "|" + y + "|" + r);
                            sphera.Close();
                        }
                        break;
                    }
                case 2:
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Нужно ввести высоту куба", "Предупреждение");
                        }
                        else
                        {
                            StreamWriter kub = new StreamWriter("Куб.txt");
                            kub.WriteLine("Исходные данные куба");
                            kub.WriteLine("Координаты Х|Координаты Y|Высота");
                            kub.WriteLine("-------------------------------");
                            kub.WriteLine(x + "|" + y + "|" + r);
                            kub.Close();
                        }
                        break;
                    }
            }
        }  

        //запись данных в файл с 4 параметрами
        public void write(double x, double y, double r, double h)
        {
            //в зависимсоти от установленного типа объекта 
            switch (comboBox1.SelectedIndex)
            {
                // записываем исходные данные выбранного объекта
                 case 1:
                     {
                         if (textBox1.Text == "" || textBox2.Text == "")
                         {
                             MessageBox.Show("Нужно ввести данные цилиндра", "Предупреждение");
                         }
                         else
                         {
                             StreamWriter cilindr = new StreamWriter("Цилиндр.txt");
                             cilindr.WriteLine("Исходные данные цилиндра");
                             cilindr.WriteLine("Координаты Х|Координаты Y|Радиус|Высота");
                             cilindr.WriteLine("--------------------------------------");
                             cilindr.WriteLine(x + "|" + y + "|" + r + "|" + h);
                             cilindr.Close();
                         }
                         break;
                     }
                case 3:
                    {
                        if (textBox1.Text == "" || textBox2.Text == "")
                        {
                            MessageBox.Show("Нужно ввести данные конуса", "Предупреждение");
                        }
                        else
                        {
                            StreamWriter konus = new StreamWriter("Конус.txt");
                            konus.WriteLine("Исходные данные конуса");
                            konus.WriteLine("Координаты Х|Координаты Y|Радиус|Высота");
                            konus.WriteLine("--------------------------------------");
                            konus.WriteLine(x + "|" + y + "|" + r + "|" + h);
                            konus.Close();
                        }
                        break;
                    }
                case 4:
                    {
                        if (textBox1.Text == "" || textBox2.Text == "")
                        {
                            MessageBox.Show("Нужно ввести данные тора", "Предупреждение");
                        }
                        else
                        {
                            StreamWriter tor = new StreamWriter("Тор.txt");
                            tor.WriteLine("Исходные данные тора");
                            tor.WriteLine("Координаты Х|Координаты Y|Наруж. диаметр|Вн. диаметр");
                            tor.WriteLine("--------------------------------------");
                            tor.WriteLine(x + "|" + y + "|" + r + "|" + h);
                            tor.Close();
                        }
                        break;
                    }
            }
        } 
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 2)
                write(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox1.Text));
            else
                write(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
            button3.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = true;
            }
            else
            {
                label4.Visible = textBox3.Visible = textBox4.Visible = button1.Visible = button3.Visible = false;
            }
        }
        
    }
}
