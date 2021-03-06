﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using PolandVisaAuto;
using pvhelper;

namespace E_Konsulat
{
    public partial class Form1 : Form
    {
        private BindingList<KonsulatTask> _tasks = new BindingList<KonsulatTask>();
        private BindingList<KonsulatTask> _completedTasks = new BindingList<KonsulatTask>();
        private Engine _engine;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateHeader();
            chbAutoResolveImage.Checked = ImageResolver.Instance.AutoResolveImage;
            radiocom.Checked = ImageResolver.Instance.Host == radiocom.Text;
            radioinfo.Checked = ImageResolver.Instance.Host == radioinfo.Text;
            chbProxy.Checked = ImageResolver.Instance.UseProxy;

//            cbxCity.DataSource = Const.GetListFromDict(Const.SettingsCities);
//            cbxNation.DataSource = Const.GetListFromDict(Const.FillNations());
//            cbxStatus.DataSource = Const.GetListFromDict(Const.FillStatus());
//            cbxPurpose.DataSource = Const.GetListFromDict(Const.FillPurpose());
//            cbxCategory.DataSource = Const.GetListFromDict(Const.GetCategoryType());
            cbxPriority.DataSource = Const.GetListPriority();

//            cityDataGridViewComboBoxColumn.DataSource = Const.GetListFromDict(Const.SettingsCities);
//            priorityComboBoxColumn.DataSource = Const.GetDataTablePriority();
//
//            cbxStatus.SelectedItem = "Mr.";
//            cbxNation.SelectedItem = "UKRAINE";

            _tasks = KonsulatTask.DeSerialize(KonsulatTaskEntityType.New);
            _engine = new Engine(_tasks, tabControl1);//, _completedVisaTasks);

            //!! 
            // _engine.SetProxy();
            _engine.RefreshViewTabs();
            _engine.ETaskEvent += _engine_ETaskEvent;

            dataGridView1.DataSource = _tasks;

        }

        private void UpdateHeader()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            this.Text += version + "   De-Captcher Balance: " + ImageResolver.Instance.GetBalance() + "$";
        }

        void _engine_ETaskEvent(KonsulatTask task)
        {
           // WriteReportFile(task);
            RemoveTask(task);
        }

        private void RemoveTask(KonsulatTask vt)
        {
            _tasks.Remove(vt);
            KonsulatTask.Save(_tasks, KonsulatTaskEntityType.New);
            _completedTasks.Add(vt);
            KonsulatTask.Save(_completedTasks, KonsulatTaskEntityType.Completed);
        }
        private void btnAddTask_Click(object sender, EventArgs e)
        {
            _tasks.Add(fillKonsulatTask());
            KonsulatTask.Save(_tasks, KonsulatTaskEntityType.New);
            _engine.RefreshViewTabs();
            dataGridView1.Refresh();
        }

        private KonsulatTask fillKonsulatTask()
        {
            KonsulatTask task = new KonsulatTask();

            //PersonalData
            task.LastNamePersonal = personalData1.LastNamePersonal;
            task.FirstNamePersonal = personalData1.FirstNamePersonal;
            task.PreviousLastNamePersonal = personalData1.PreviousLastNamePersonal;
            task.DobPersonal = personalData1.DobPersonal;
            task.DobCityPersonal = personalData1.DobCityPersonal;
            task.DobCountryPersonal = personalData1.DobCountryPersonal;
            task.CitizenshipPersonal = personalData1.CitizenshipPersonal;
            task.NationalityPersonal = personalData1.NationalityPersonal;
            task.SexMRadioPersonal = personalData1.SexMRadioPersonal;
            task.FamilyStateRadioPersonal = personalData1.FamilyStateRadioPersonal;


            //PassportData
            task.PassportNumber = passportData1.PassportNumber;
            task.PassportFromDate = passportData1.PassportFromDate;
            task.PassportToDate = passportData1.PassportToDate;
            task.PassportSource = passportData1.PassportSource;
            task.PassportTypeRadio = passportData1.PassportTypeRadio;


            //ChildData
            task.CitizenshipChild = childData1.CitizenshipChild;
            task.FirstNameChild = childData1.FirstNameChild;
            task.LastNameChild = childData1.LastNameChild;
            task.StateChild = childData1.StateChild;
            task.CityChild = childData1.CityChild;
            task.AddressChild = childData1.AddressChild;
            task.CountryChild = childData1.CountryChild;
            task.ZipChild = childData1.ZipChild;

            //AddressData
            task.CountryAddress = addressData1.CountryAddress;
            task.CityAddress = addressData1.CityAddress;
            task.StateAddress = addressData1.StateAddress;
            task.ZipAddress = addressData1.ZipAddress;
            task.AddressAddress = addressData1.AddressAddress;
            task.EmailAddress = addressData1.EmailAddress;
            task.PrefixAddress = addressData1.PrefixAddress;
            task.TelNumberAddress = addressData1.TelNumberAddress;

            //BossAddress
            task.CountryBossAddress = bossAddressData1.CityBossAddress;
            task.CityBossAddresss = bossAddressData1.CityBossAddress;
            task.StateBossAddress = bossAddressData1.StateBossAddress;
            task.ZipBossAddress = bossAddressData1.ZipBossAddress;
            task.AddressBossAddress = bossAddressData1.AddressBossAddress;
            task.EmailBossAddress = bossAddressData1.EmailBossAddress;
            task.PrefixBossAddress = bossAddressData1.PrefixBossAddress;
            task.TelNumberBossAddress = bossAddressData1.TelNumberBossAddress;
            task.FaxBossAddress = bossAddressData1.FaxBossAddress;
            task.FaxPrefixBossAddress = bossAddressData1.FaxBossAddress;
            task.BLRadioBossAddress = bossAddressData1.BLRadioBossAddress;

            //Fingerprints
            task.FingYNFingerprints = fingerprintsData1.FingYNFingerprints;

            //TheseEuCitizen
            task.NameEuCitizen = theseEUCitizenData1.NameEuCitizen;
            task.LastNameEuCitizen = theseEUCitizenData1.LastNameEuCitizen;
            task.DobEuCitizen = theseEUCitizenData1.DobEuCitizen;
            task.PassportNumberEuCitizen = theseEUCitizenData1.PassNumberEuCitizen;
            task.DobEuCitizen = theseEUCitizenData1.DobEuCitizen;


            //TargetData
            task.TourismTarget = targetData1.TourismTarget;
            task.FamilyTarget = targetData1.FamilyTarget;
            task.SportTarget = targetData1.SportTarget;
            task.LearningTarget = targetData1.LearningTarget;
            task.TreatmentTarget = targetData1.TreatmentTarget;
            task.OfficialTarget = targetData1.OfficialTarget;
            task.OtherTarget = targetData1.OtherTarget;
            task.TextOtherTarget = targetData1.TextOtherTarget;
            task.CultureTarget = targetData1.CultureTarget;
            task.DealTarget = targetData1.DealTarget;


            //InformationData
            task.EntryStateRadioInform = informationData1.EntryStateRadioInform;
            task.CheckOutInform = informationData1.CheckOutInform;
            task.ArrivalInform = informationData1.ArrivalInform;
            task.DestinationInform = informationData1.DestinationInform;
            task.NumbDayInform = informationData1.NumbDayInform;
            task.WaiverInform = informationData1.WaiverInform;


            //SchengenvisasData
            task.YNVisa = schengen_visasData1.YNVisa;
            task.From1Visa = schengen_visasData1.From1Visa;
            task.From2Visa = schengen_visasData1.From2Visa;
            task.To1Visa = schengen_visasData1.To1Visa;
            task.To2Visa = schengen_visasData1.To2Visa;

            //InfereceivingData
            task.FirmManRadioInferec = infreceivingData1.FirmManRadioInferec;
            task.TitleInferec = infreceivingData1.TitleInferec;
            task.NameInferec = infreceivingData1.NameInferec;
            task.LastNameInferec = infreceivingData1.LastNameInferec;
            task.StateInferec = infreceivingData1.StateInferec;
            task.CityInferec = infreceivingData1.CityInferec;
            task.PostcodeInferec = infreceivingData1.PostcodeInferec;
            task.PrefixInferec = infreceivingData1.PrefixInferec;
            task.TelNumberInferec = infreceivingData1.TelNumberInferec;
            task.FaxPrefixInferec = infreceivingData1.FaxPrefixInferec;
            task.FaxInferec = infreceivingData1.FaxInferec;
            task.AddressInferec = infreceivingData1.AddressInferec;
            task.HouseNumberInferec = infreceivingData1.HouseNumberInferec;
            task.ApNumberInferec = infreceivingData1.ApNumberInferec;
            task.EmailInferec = infreceivingData1.EmailInferec;

            //CountryData
            task.DocumentRadioCountry = countryData1.DocumentRadioCountry;
            task.FromCountry = countryData1.FromCountry;
            task.PassNumber = countryData1.PassNumber;

            //PersonalCostsData
            task.CostRadioPersCost = personsCostsData1.CostRadioPersCost;
            task.AllCostsPersCost = personsCostsData1.AllCostsPersCost;
            task.CardPersCost = personsCostsData1.CardPersCost;
            task.ChecksPersCost = personsCostsData1.ChecksPersCost;
            task.DateInsurancePersCost = personsCostsData1.DateInsurancePersCost;
            task.InsurancePersCost = personsCostsData1.InsurancePersCost;
            task.LocationPersCost = personsCostsData1.LocationPersCost;
            task.MoneyPersCost = personsCostsData1.MoneyPersCost;
            task.OtheCostsPersCost = personsCostsData1.OtheCostsPersCost;
            task.OthePersCost = personsCostsData1.OthePersCost;
            task.OtherCostsPersCost = personsCostsData1.OtherCostsPersCost;
            task.OtherPersCost = personsCostsData1.OtherPersCost;
            task.ReferredPersCost = personsCostsData1.ReferredPersCost;
            task.TransportPersCost = personsCostsData1.TransportPersCost;
            return task;
        }

        private void fillDataToControlFromKonsulatTask(KonsulatTask task)
        {
            //PersonalData
            personalData1.LastNamePersonal = task.LastNamePersonal;
            personalData1.FirstNamePersonal = task.FirstNamePersonal;
            personalData1.PreviousLastNamePersonal = task.PreviousLastNamePersonal;
            personalData1.DobPersonal = task.DobPersonal;
            personalData1.DobCityPersonal = task.DobCityPersonal;
            personalData1.DobCountryPersonal = task.DobCountryPersonal;
            personalData1.CitizenshipPersonal = task.CitizenshipPersonal;
            personalData1.NationalityPersonal = task.NationalityPersonal;
            personalData1.SexMRadioPersonal = task.SexMRadioPersonal;
            personalData1.FamilyStateRadioPersonal = task.FamilyStateRadioPersonal;


            //PassportData
            passportData1.PassportNumber = task.PassportNumber;
            passportData1.PassportFromDate = task.PassportFromDate;
            passportData1.PassportToDate = task.PassportToDate;
            passportData1.PassportSource = task.PassportSource;
            passportData1.PassportTypeRadio = task.PassportTypeRadio;


            //ChildData
            childData1.CitizenshipChild = task.CitizenshipChild;
            childData1.FirstNameChild = task.FirstNameChild;
            childData1.LastNameChild = task.LastNameChild;
            childData1.StateChild = task.StateChild;
            childData1.CityChild = task.CityChild;
            childData1.AddressChild = task.AddressChild;
            childData1.CountryChild = task.CountryChild;
            childData1.ZipChild = task.ZipChild;

            //AddressData
            addressData1.CountryAddress = task.CountryAddress;
            addressData1.CityAddress = task.CityAddress;
            addressData1.StateAddress = task.StateAddress;
            addressData1.ZipAddress = task.ZipAddress;
            addressData1.AddressAddress = task.AddressAddress;
            addressData1.EmailAddress = task.EmailAddress;
            addressData1.PrefixAddress = task.PrefixAddress;
            addressData1.TelNumberAddress = task.TelNumberAddress;

            //BossAddress
            bossAddressData1.CityBossAddress = task.CountryBossAddress;
            bossAddressData1.CityBossAddress = task.CityBossAddresss;
            bossAddressData1.StateBossAddress = task.StateBossAddress;
            bossAddressData1.ZipBossAddress = task.ZipBossAddress;
            bossAddressData1.AddressBossAddress = task.AddressBossAddress;
            bossAddressData1.EmailBossAddress = task.EmailBossAddress;
            bossAddressData1.PrefixBossAddress = task.PrefixBossAddress;
            bossAddressData1.TelNumberBossAddress = task.TelNumberBossAddress;
            bossAddressData1.FaxBossAddress = task.FaxBossAddress;
            bossAddressData1.FaxBossAddress = task.FaxPrefixBossAddress;
            bossAddressData1.BLRadioBossAddress = task.BLRadioBossAddress;

            //Fingerprints
            fingerprintsData1.FingYNFingerprints = task.FingYNFingerprints;

            //TheseEuCitizen
            theseEUCitizenData1.NameEuCitizen = task.NameEuCitizen;
            theseEUCitizenData1.LastNameEuCitizen = task.LastNameEuCitizen;
            theseEUCitizenData1.DobEuCitizen = task.DobEuCitizen;
            theseEUCitizenData1.PassNumberEuCitizen = task.PassportNumberEuCitizen;
            theseEUCitizenData1.DobEuCitizen = task.DobEuCitizen;


            //TargetData
            targetData1.TourismTarget = task.TourismTarget;
            targetData1.FamilyTarget = task.FamilyTarget;
            targetData1.SportTarget = task.SportTarget;
            targetData1.LearningTarget = task.LearningTarget;
            targetData1.TreatmentTarget = task.TreatmentTarget;
            targetData1.OfficialTarget = task.OfficialTarget;
            targetData1.OtherTarget = task.OtherTarget;
            targetData1.TextOtherTarget = task.TextOtherTarget;
            targetData1.CultureTarget = task.CultureTarget;
            targetData1.DealTarget = task.DealTarget;


            //InformationData
            informationData1.EntryStateRadioInform = task.EntryStateRadioInform;
            informationData1.CheckOutInform = task.CheckOutInform;
            informationData1.ArrivalInform = task.ArrivalInform;
            informationData1.DestinationInform = task.DestinationInform;
            informationData1.NumbDayInform = task.NumbDayInform;
            informationData1.WaiverInform = task.WaiverInform;


            //SchengenvisasData
            schengen_visasData1.YNVisa = task.YNVisa;
            schengen_visasData1.From1Visa = task.From1Visa;
            schengen_visasData1.From2Visa = task.From2Visa;
            schengen_visasData1.To1Visa = task.To1Visa;
            schengen_visasData1.To2Visa = task.To2Visa;

            //InfereceivingData
            infreceivingData1.FirmManRadioInferec = task.FirmManRadioInferec;
            infreceivingData1.TitleInferec = task.TitleInferec;
            infreceivingData1.NameInferec = task.NameInferec;
            infreceivingData1.LastNameInferec = task.LastNameInferec;
            infreceivingData1.StateInferec = task.StateInferec;
            infreceivingData1.CityInferec = task.CityInferec;
            infreceivingData1.PostcodeInferec = task.PostcodeInferec;
            infreceivingData1.PrefixInferec = task.PrefixInferec;
            infreceivingData1.TelNumberInferec = task.TelNumberInferec;
            infreceivingData1.FaxPrefixInferec =  task.FaxPrefixInferec;
            infreceivingData1.FaxInferec = task.FaxInferec;
            infreceivingData1.AddressInferec = task.AddressInferec;
            infreceivingData1.HouseNumberInferec = task.HouseNumberInferec;
            infreceivingData1.ApNumberInferec = task.ApNumberInferec;
            infreceivingData1.EmailInferec = task.EmailInferec;

            //CountryData
            countryData1.DocumentRadioCountry = task.DocumentRadioCountry;
            countryData1.FromCountry = task.FromCountry;
            countryData1.PassNumber = task.PassNumber;

            //PersonalCostsData
            personsCostsData1.CostRadioPersCost = task.CostRadioPersCost;
            personsCostsData1.AllCostsPersCost = task.AllCostsPersCost;
            personsCostsData1.CardPersCost = task.CardPersCost;
            personsCostsData1.ChecksPersCost = task.ChecksPersCost;
            personsCostsData1.DateInsurancePersCost = task.DateInsurancePersCost;
            personsCostsData1.InsurancePersCost = task.InsurancePersCost;
            personsCostsData1.LocationPersCost = task.LocationPersCost;
            personsCostsData1.MoneyPersCost = task.MoneyPersCost;
            personsCostsData1.OtheCostsPersCost = task.OtheCostsPersCost;
            personsCostsData1.OthePersCost = task.OthePersCost;
            personsCostsData1.OtherCostsPersCost = task.OtherCostsPersCost;
            personsCostsData1.OtherPersCost = task.OtherPersCost;
            personsCostsData1.ReferredPersCost = task.ReferredPersCost;
            personsCostsData1.TransportPersCost = task.TransportPersCost;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KonsulatTask task = dataGridView1.Rows[e.RowIndex].DataBoundItem as KonsulatTask;
            fillDataToControlFromKonsulatTask(task);

            DialogForm form = new DialogForm();
            form.Text = "Редактирование задачи";
            form.Size = new Size(panel1.Size.Width, panel1.Size.Height+50);
            form.Controls.Add(okCancelControl1);
            form.Controls.Add(panel1);
            okCancelControl1.SendToBack();
            okCancelControl1.OnOk += form.okCancelControl1_OnOk;
            okCancelControl1.OnCancel += form.okCancelControl1_OnCancel;
            groupBox6.Visible = groupBox7.Visible = false;
            okCancelControl1.Visible = true;
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _tasks.Remove(task);
                var newTask = fillKonsulatTask();
                _tasks.Add(newTask);
                dataGridView1.Refresh();
                KonsulatTask.Save(_tasks, KonsulatTaskEntityType.New);
            }
            groupBox6.Visible = groupBox7.Visible = true;
            okCancelControl1.Visible = false;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }
    }
}
