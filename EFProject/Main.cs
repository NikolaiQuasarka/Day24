using EFProject.EF;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
namespace EFProject
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            using (HotelContext hotelContext = new HotelContext())
            {
                foreach (Room room in hotelContext.Rooms)
                {
                    dataGridView1.Rows.Add(room.IdRoom, room.Number, room.IdGuest, room.GuestCount, room.ComfortLevel, room.Price, room.IdGuestNavigation);
                }
                comboBoxChangeClient.DataSource = hotelContext.Guests.ToList();
                comboBoxChangeClient.DisplayMember = "FullName";
                comboBoxChangeClient.ValueMember = "IdGuest";
            }
        }
        private void Add(Room newRoom)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                if (!hotelContext.Rooms.Any(x => x.Number == newRoom.Number))
                {
                    hotelContext.Rooms.Add(newRoom);
                    hotelContext.SaveChanges();
                    Room room = hotelContext.Rooms.FirstOrDefault(x => x.IdRoom == newRoom.IdRoom);
                    dataGridView1.Rows.Add(room.IdRoom, room.Number, room.IdGuest, room.GuestCount, room.ComfortLevel, room.Price, room.IdGuestNavigation);

                }
            }
        }
        private void Delete(int id)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                hotelContext.Rooms.Remove(hotelContext.Rooms.FirstOrDefault(x => x.IdRoom == id));
                hotelContext.SaveChanges();
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
        }
        private void Update(int id, int number, int? idGuest, int countGuest, int level, int price)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                Room changingRoom = hotelContext.Rooms.FirstOrDefault(x => x.IdRoom == id);

                changingRoom.Number = number;
                changingRoom.Price = price;
                changingRoom.IdGuest = idGuest;
                changingRoom.ComfortLevel = level;
                changingRoom.GuestCount = countGuest;
                hotelContext.SaveChanges();

                dataGridView1.SelectedRows[0].Cells[1].Value=number;
                dataGridView1.SelectedRows[0].Cells[2].Value = idGuest;
                dataGridView1.SelectedRows[0].Cells[3].Value = countGuest;
                dataGridView1.SelectedRows[0].Cells[4].Value = level;
                dataGridView1.SelectedRows[0].Cells[5].Value = price;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxNumber.Text, out int num) & int.TryParse(textBoxComfortLevel.Text, out int level) & int.TryParse(textBoxPrice.Text, out int price))
            {
                Room newRoom = new Room()
                {
                    ComfortLevel = level,
                    Price = price,
                    Number = num
                };
                Add(newRoom);
            }
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                hotelContext.SaveChanges();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                Delete(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString()));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddGuestForm addGuestForm = new AddGuestForm();
            addGuestForm.FormClosed += delegate (object? sender, FormClosedEventArgs e)
            {
                using (HotelContext hotelContext = new HotelContext()) {
                    comboBoxChangeClient.DataSource = hotelContext.Guests.ToList();
                };
            };
            addGuestForm.ShowDialog();
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1&
                int.TryParse(textBoxChangeNumber.Text, out int chNum)&int.TryParse(textBoxChangePrice.Text, out int price)&
                int.TryParse(textBoxChangeComfortLevel.Text,out int level)& int.TryParse(textBoxChangeCountGuest.Text, out int count))
            {
                Update(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value), chNum, Convert.ToInt32(comboBoxChangeClient.SelectedValue), count, level, price);
            }
        }
    }
}