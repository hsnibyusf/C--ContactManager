using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using training3.ManagerContext;
using training3.Models;

namespace training3
{
    public partial class Form1 : Form
    {
        private readonly CrudManager crudManager;
        private Contact selectedContact;

        public Form1()
        {
            InitializeComponent();

            ManagerContext.dbContext dbContext = new ManagerContext.dbContext();
            crudManager = new CrudManager(dbContext);
            pnlAddContact.Visible = true;
            pnlSearchContact.Visible = false;
            ContactPhonePanel.Visible = false;
            addresspanel.Visible = false;
            Emailpanel.Visible = false;
            HeaderNewLabel.Font = new Font(HeaderNewLabel.Font, FontStyle.Bold);
            List<Contact> allContacts = crudManager.GetAllContacts();
            dgvSearchResults.AutoGenerateColumns = false;
            dgvSearchResults.DataSource = allContacts;
            PhoneAddbtn.Click += (s, ev) => AddPhone();
            AddressAddbtn.Click += (s, ev) => AddAddress();
            EmailAddbtn.Click += (s, v) => AddEmail();

            emailgrid.CellContentClick += (sender, e) =>
            {
                if (e.ColumnIndex == emailgrid.Columns["DeleteEmail"].Index && e.RowIndex >= 0)
                {
                    int emailId = (int)emailgrid.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn3"].Value;
                    DeleteEmailAddress(emailId);
                    LoadEmail(selectedContact.Id);
                }
            };
            dataGridView1.CellContentClick += (sender, e) =>
            {
               
                if (e.ColumnIndex == dataGridView1.Columns["DeletePhone"].Index && e.RowIndex >= 0)
                {           
                    int phoneId = (int)dataGridView1.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn2"].Value;

                    DeletePhoneNumber(phoneId);

                    LoadPhone(selectedContact.Id);
                }
            };
            AddressGrid.CellContentClick += (sender, e) =>
            {
              
                if (e.ColumnIndex == AddressGrid.Columns["AddressDelete"].Index && e.RowIndex >= 0)
                {
                    
                    int addressId = (int)AddressGrid.Rows[e.RowIndex].Cells["idDataGridVaiewTextBoxColumn1"].Value;
                    
                    DeleteAddressValue(addressId);

                 
                    LoadAddress(selectedContact.Id);
                }
            };

        }
        private bool IsValidName(string name)
        {

            string namePattern = @"^[a-zA-Z]+$";
            return Regex.IsMatch(name, namePattern);
        }

        private void Malechbx_CheckedChanged(object sender, EventArgs e)
        {

            if (Malechbx.Checked)
                Femalechbx.Checked = false;
        }

        private void Femalechbx_CheckedChanged(object sender, EventArgs e)
        {
            if (Femalechbx.Checked)
                Malechbx.Checked = false;
        }
        private bool IsValidEmail(string email)
        {

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPhone(string phone)
        {

            string phonePattern = @"^\d{7}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        private bool IsValidAddress(string address)
        {

            string addressPattern = @"^\w+-\w+-\w+-\w+$";
            return Regex.IsMatch(address, addressPattern);
        }

        private bool IsValidType(string type)
        {

            string typePattern = @"^[a-zA-Z]+$";
            return Regex.IsMatch(type, typePattern);
        }
        private void AddEmail()
        {
            if (string.IsNullOrWhiteSpace(Emailtxtbox.Text) || string.IsNullOrWhiteSpace(EmailTypetxtbox.Text))
            {
                MessageBox.Show("Please enter email number and type.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(Emailtxtbox.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Email NewEmail = new Email
            {
                EmailAddress = Emailtxtbox.Text,
                Type = EmailTypetxtbox.Text,
                ContactId = selectedContact.Id,
                ContactName = selectedContact.FullName
            };


            crudManager.AddEmail(NewEmail);


            LoadEmail(selectedContact.Id);


            Emailtxtbox.Text = string.Empty;
            EmailTypetxtbox.Text = string.Empty;
        }
        private void LoadEmail(int contactId)
        {

            List<Email> emails = crudManager.GetAllEmails(contactId);

            emailgrid.AutoGenerateColumns = false;
            emailgrid.DataSource = emails;
        }
        private void AddAddress()
        {
            if (string.IsNullOrWhiteSpace(Addresstxtbox.Text) || string.IsNullOrWhiteSpace(AddressTypetxtbox.Text))
            {
                MessageBox.Show("Please enter Address number and type.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidAddress(Addresstxtbox.Text))
            {
                MessageBox.Show("Please enter a valid address in the format: country-city-street-building.", "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidType(AddressTypetxtbox.Text))
            {
                MessageBox.Show("Please enter a valid type for the address.", "Invalid Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Address NewAddress = new Address
            {
                AddressValue = Addresstxtbox.Text,
                Type = AddressTypetxtbox.Text,
                ContactId = selectedContact.Id,
                ContactName = selectedContact.FullName
            };


            crudManager.AddAddress(NewAddress);


            LoadAddress(selectedContact.Id);


            Addresstxtbox.Text = string.Empty;
            AddressTypetxtbox.Text = string.Empty;
        }
        private void LoadAddress(int contactId)
        {

            List<Address> addresses = crudManager.GetAllAddresses(contactId);


            AddressGrid.AutoGenerateColumns = false;
            AddressGrid.DataSource = addresses;
        }
        private void AddPhone()
        {
            if (string.IsNullOrWhiteSpace(phonetxtbox.Text) || string.IsNullOrWhiteSpace(typetxtbox.Text))
            {
                MessageBox.Show("Please enter phone number and type.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhone(phonetxtbox.Text))
            {
                MessageBox.Show("Please enter a valid 7-digit phone number.", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Phone newPhone = new Phone
            {
                PhoneNumber = phonetxtbox.Text,
                Type = typetxtbox.Text,
                ContactId = selectedContact.Id,
                ContactName = selectedContact.FullName
            };


            crudManager.AddPhone(newPhone);


            LoadPhone(selectedContact.Id);


            phonetxtbox.Text = string.Empty;
            typetxtbox.Text = string.Empty;
        }

        private void LoadPhone(int contactId)
        {

            List<Phone> phones = crudManager.GetPhonesByContactId(contactId);


            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = phones;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
           
            if (!IsValidName(txtFirstName.Text) ||
                !IsValidName(txtMiddleName.Text) ||
                !IsValidName(txtLastName.Text) ||
                (!Malechbx.Checked && !Femalechbx.Checked))
            {
                MessageBox.Show("Please enter valid first name, middle name, last name, and select gender.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Contact newContact = new Contact();
            newContact.FullName = $"{txtFirstName.Text} {txtMiddleName.Text} {txtLastName.Text}";
            newContact.DateOfBirth = dtpDateOfBirth.Value;

            if (Malechbx.Checked)
                newContact.Gender = "Male";
            else if (Femalechbx.Checked)
                newContact.Gender = "Female";

            crudManager.AddContact(newContact);

            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            dtpDateOfBirth.Value = DateTime.Now;
            Malechbx.Checked = false;
            Femalechbx.Checked = false;

            dgvSearchResults.DataSource = null;

            MessageBox.Show("Contact added successfully!");
        }
        private void HeaderSearchLabel_Click(object sender, EventArgs e)
        {
            pnlAddContact.Visible = false;
            pnlSearchContact.Visible = true;
            ContactPhonePanel.Visible = false;
            addresspanel.Visible = false;
            Emailpanel.Visible = false;
            HeaderNewLabel.Font = new Font(HeaderNewLabel.Font, FontStyle.Regular);
            HeaderSearchLabel.Font = new Font(HeaderSearchLabel.Font, FontStyle.Bold);

        }

        private void HeaderNewLabel_Click(object sender, EventArgs e)
        {
            pnlAddContact.Visible = true;
            pnlSearchContact.Visible = false;
            ContactPhonePanel.Visible = false;
            addresspanel.Visible = false;
            Emailpanel.Visible = false;
            HeaderNewLabel.Font = new Font(HeaderNewLabel.Font, FontStyle.Bold);
            HeaderSearchLabel.Font = new Font(HeaderSearchLabel.Font, FontStyle.Regular);
            btnSave.Visible = true;
            Updatebtn.Visible = false;
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchTerm.Text;

            List<Contact> searchResults = crudManager.SearchContactsByName(searchTerm);
            dgvSearchResults.DataSource = null;
            dgvSearchResults.DataSource = searchResults;

            if (searchResults.Count == 0)
            {
                MessageBox.Show("No contacts found matching the search criteria.");
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            DateTime? startDate = dtpStartDate.Value.Date;
            DateTime? endDate = dtpEndDate.Value.Date;

            List<Contact> searchResults = crudManager.SearchContactsByDates(startDate, endDate);
            dgvSearchResults.DataSource = null;
            dgvSearchResults.DataSource = searchResults;

            if (searchResults.Count == 0)
            {
                MessageBox.Show("No contacts found matching the search criteria.");
            }
        }

        private void dgvSearchResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int contactId = (int)dgvSearchResults.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value;
                selectedContact = crudManager.GetContactById(contactId);

                if (selectedContact != null)
                {
                    if (e.ColumnIndex == dgvSearchResults.Columns["idDataGridViewTextBoxColumn"].Index)
                    {
                        LoadContactDetails(selectedContact);
                    }
                    else if (e.ColumnIndex == dgvSearchResults.Columns["Email"].Index)
                    {
                        ShowEmailPanel();
                    }
                    else if (e.ColumnIndex == dgvSearchResults.Columns["Phone"].Index)
                    {
                        ShowPhonePanel();
                    }
                    else if (e.ColumnIndex == dgvSearchResults.Columns["Address"].Index)
                    {
                        ShowAddressPanel();
                    }
                    else if (e.ColumnIndex == dgvSearchResults.Columns["Delete"].Index)
                    {
                        DeleteContact(contactId);
                    }

                }
            }
        }



        private void LoadContactDetails(Contact contact)
        {
            string[] fullName = contact.FullName.Split(' ');
            txtFirstName.Text = fullName.Length > 0 ? fullName[0] : string.Empty;
            txtMiddleName.Text = fullName.Length > 1 ? fullName[1] : string.Empty;
            txtLastName.Text = fullName.Length > 2 ? fullName[2] : string.Empty;
            dtpDateOfBirth.Value = contact.DateOfBirth.HasValue ? contact.DateOfBirth.Value : DateTime.Now;

            if (contact.Gender == "Male")
                Malechbx.Checked = true;
            else if (contact.Gender == "Female")
                Femalechbx.Checked = true;

            pnlAddContact.Visible = true;
            pnlSearchContact.Visible = false;
            HeaderNewLabel.Font = new Font(HeaderNewLabel.Font, FontStyle.Bold);
            HeaderSearchLabel.Font = new Font(HeaderSearchLabel.Font, FontStyle.Regular);
            btnSave.Visible = false;
            Updatebtn.Visible = true;
        }

        private void ShowEmailPanel()
        {
            pnlAddContact.Visible = false;
            pnlSearchContact.Visible = false;
            ContactPhonePanel.Visible = false;
            addresspanel.Visible = false;
            Emailpanel.Visible = true;
            LoadEmail(selectedContact.Id);
        }

        private void ShowPhonePanel()
        {
            pnlAddContact.Visible = false;
            pnlSearchContact.Visible = false;
            ContactPhonePanel.Visible = true;
            addresspanel.Visible = false;
            Emailpanel.Visible = false;
            LoadPhone(selectedContact.Id);
        }

        private void ShowAddressPanel()
        {
            pnlAddContact.Visible = false;
            pnlSearchContact.Visible = false;
            ContactPhonePanel.Visible = false;
            addresspanel.Visible = true;
            Emailpanel.Visible = false;
            LoadAddress(selectedContact.Id);
        }

        private void DeleteContact(int contactId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this contact?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                crudManager.DeleteContact(contactId);

                if (pnlSearchContact.Visible)
                {
                    btnSearch2_Click(this, EventArgs.Empty);
                }

                MessageBox.Show("Contact deleted successfully!");
                ClearForm();
            }
        }


        private void ClearForm()
        {
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            dtpDateOfBirth.Value = DateTime.Now;
            Malechbx.Checked = false;
            Femalechbx.Checked = false;
        }
        private void btnDeleteEmail_Click(object sender, EventArgs e)
        {
            if (emailgrid.SelectedRows.Count > 0)
            {
                // Assuming the email ID is in a column named "idDataGridViewTextBoxColumn3"
                int emailId = (int)emailgrid.SelectedRows[0].Cells["idDataGridViewTextBoxColumn3"].Value;
                DeleteEmailAddress(emailId);
            }
        }
        private void btnDeletePhone_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                
                int phoneId = (int)dataGridView1.SelectedRows[0].Cells["idDataGridViewTextBoxColumn2"].Value;
                DeletePhoneNumber(phoneId);
            }
        }
        private void btnDeleteAddress_Click(object sender, EventArgs e)
        {
            if (AddressGrid.SelectedRows.Count > 0)
            {

                int addressId = (int)dataGridView1.SelectedRows[0].Cells["idDataGridVaiewTextBoxColumn1"].Value;
                DeleteAddressValue(addressId);
            }
        }
        private void DeleteEmailAddress(int emailId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this email?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                crudManager.DeleteEmail(emailId);
                MessageBox.Show("Email deleted successfully!");
                LoadEmail(selectedContact.Id); 
            }
        }
        private void DeletePhoneNumber(int phoneId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this phone?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                crudManager.DeletePhone(phoneId);
                MessageBox.Show("phone deleted successfully!");
                LoadPhone(selectedContact.Id);
            }
        }
        private void DeleteAddressValue(int addressId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this address?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                crudManager.AddressDelete(addressId);
                MessageBox.Show("address deleted successfully!");
                LoadAddress(selectedContact.Id);
            }
        }
        private void Updatebtn_Click(object sender, EventArgs e)
        {
            if (selectedContact != null)
            {

                selectedContact.FullName = $"{txtFirstName.Text} {txtMiddleName.Text} {txtLastName.Text}";
                selectedContact.DateOfBirth = dtpDateOfBirth.Value;

                if (Malechbx.Checked)
                    selectedContact.Gender = "Male";
                else if (Femalechbx.Checked)
                    selectedContact.Gender = "Female";


                crudManager.EditContact(selectedContact.Id, selectedContact);

                MessageBox.Show("Contact updated successfully!");



                ClearForm();


                if (pnlSearchContact.Visible)
                {

                    btnSearch2_Click(sender, EventArgs.Empty);
                }
            }
            else
            {
                MessageBox.Show("No contact selected for update.");
            }
        }

       
    }
}
