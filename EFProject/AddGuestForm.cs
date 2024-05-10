using EFProject.EF;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFProject
{
    public partial class AddGuestForm : Form
    {
        public AddGuestForm()
        {
            InitializeComponent();

            using (HotelContext hotelContext = new HotelContext())
            {
                FullUpdate(hotelContext.Guests);
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (File.Exists(openFileDialog1.FileName))
            {
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }


        private void AddGuestForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                Delete(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            }
        }
        private void Delete(int id)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                hotelContext.Guests.Remove(hotelContext.Guests.FirstOrDefault(x => x.IdGuest == id));
                hotelContext.SaveChanges();
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
        }
        private void buttonAddClient_Click(object sender, EventArgs e)
        {
            if (textBoxFullName.Text != "" & comboBoxGender.Text != "")
            {
                Guest newGuest = new Guest()
                {
                    FullName = textBoxFullName.Text,
                    Gender = comboBoxGender.Text,
                    BirthDay = DateOnly.FromDateTime(dateTimePickerBirthDay.Value)                  
                };
                if(File.Exists(openFileDialog1.FileName))
                {
                    newGuest.FaceImage = File.ReadAllBytes(openFileDialog1.FileName);
                }
                Add(newGuest);
            }
        }
        private void Add(Guest newGuest)
        {
            using (HotelContext newHotelContext = new HotelContext())
            {
                newHotelContext.Guests.Add(newGuest);
                newHotelContext.SaveChanges();
                Guest addedGuest = newHotelContext.Guests.FirstOrDefault(x => x.IdGuest == newGuest.IdGuest);
                dataGridView1.Rows.Add(addedGuest.IdGuest, addedGuest.FullName, addedGuest.BirthDay, addedGuest.Gender, addedGuest.FaceImage);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                IQueryable<Guest> sortedData = null;
                if (checkBoxFullName.Checked)
                {
                    sortedData = hotelContext.Guests.Where(x => x.FullName.Contains(textBoxSearchFullName.Text));
                }
                if (checkBoxAge.Checked&DateOnly.TryParse(textBoxSearchAge.Text, out DateOnly date))
                {
                    if (sortedData != null)
                    {
                        sortedData = sortedData.Where(x => x.BirthDay > DateOnly.FromDateTime(dateTimePickerBirthDay.Value));
                    }
                    else sortedData = hotelContext.Guests.Where(x => x.BirthDay > date);
                }
                if (sortedData != null)
                {
                    var sortedList = sortedData.ToList();
                    dataGridView1.Rows.Clear();
                    FullUpdate(sortedList);
                }
            }

        }
        private void FullUpdate(IEnumerable<Guest> list) 
        {
            foreach (Guest guest in list)
            {
                dataGridView1.Rows.Add(guest.IdGuest, guest.FullName, guest.BirthDay, guest.Gender, guest.FaceImage);
            }
        }
        private void buttonSearchCamcel_Click(object sender, EventArgs e)
        {
            using (HotelContext hotelContext = new HotelContext())
            {
                FullUpdate(hotelContext.Guests);
            }
        }
    }
}
