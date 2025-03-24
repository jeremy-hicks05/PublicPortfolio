using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace MTAIntranet.Shared
{
    public partial class EAMProdContext : DbContext
    {
        // SQL 03
        public EAMProdContext() { }

        public EAMProdContext(DbContextOptions<EAMProdContext> options)
            : base(options)
        {
        }

        //string fmVehQuery = "select distinct VEHICLEID from dbo.MainTrans
        //  where vehicleID < 10000 AND LEN(VEHICLEID) > 7 order by VEHICLEID";

        //[DisplayName("Main Transactions")]
        //public virtual DbSet<MainTrans>? MainTrans { get; set; }

        public virtual DbSet<EqMain> EqMains { get; set; } = null!;

        public virtual DbSet<User>? User { get; set; }

        public virtual DbSet<MaxQueue>? MaxQueues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaxQueue>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("MAXQ_ErrorHandler", "maxqueue");
            });

            modelBuilder.Entity<EqMain>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EQ_MAIN", "emsdba");



                entity.HasIndex(e => new { e.AssetNo, e.EqEquipNo }, "EQ_ASSET")
                    .IsUnique();

                entity.HasIndex(e => new { e.AssetType, e.ProcstProcStatus }, "EQ_ASSET_TYPE");

                entity.HasIndex(e => new { e.EqcatEquipCategory, e.EqEquipNo }, "EQ_CATEGORY")
                    .IsUnique();

                entity.HasIndex(e => new { e.ClassClassMaint, e.EqEquipNo }, "EQ_CLASS_MAINT")
                    .IsUnique();

                entity.HasIndex(e => e.ClassClassMeter, "EQ_CLASS_METER");

                entity.HasIndex(e => new { e.ClassClassPm, e.EqEquipNo }, "EQ_CLASS_PM")
                    .IsUnique();

                entity.HasIndex(e => new { e.ClassClassRental, e.EqEquipNo }, "EQ_CLASS_RENTAL")
                    .IsUnique();

                entity.HasIndex(e => new { e.ClassClassShopSch, e.EqEquipNo }, "EQ_CLASS_SHOP_SCH")
                    .IsUnique();

                entity.HasIndex(e => new { e.ClassClassStds, e.EqEquipNo }, "EQ_CLASS_STDS")
                    .IsUnique();

                entity.HasIndex(e => new { e.DeptDeptCode, e.EqEquipNo }, "EQ_DEPT")
                    .IsUnique();

                entity.HasIndex(e => e.EqEquipNo, "EQ_EQUIPNO")
                    .IsUnique();

                entity.HasIndex(e => new { e.FltFleetNo, e.EqEquipNo }, "EQ_FLEET")
                    .IsUnique();

                entity.HasIndex(e => new { e.FuelCardNo, e.EqEquipNo }, "EQ_FUEL_CARD")
                    .IsUnique();

                entity.HasIndex(e => new { e.LicenseNo, e.EqEquipNo }, "EQ_LICNO")
                    .IsUnique();

                entity.HasIndex(e => e.LocCurrentLoc, "EQ_LOC_CUR");

                entity.HasIndex(e => new { e.LocAssignPmLoc, e.EqEquipNo }, "EQ_LOC_PM")
                    .IsUnique();

                entity.HasIndex(e => e.LocAssignReprLoc, "EQ_LOC_REPAIR");

                entity.HasIndex(e => e.DeptTempLoanedTo, "EQ_MAIN_DEPT_LOANED");

                entity.HasIndex(e => e.RowId, "EQ_MAIN_ROW")
                    .IsUnique();

                entity.HasIndex(e => new { e.OperOperNo, e.EqEquipNo }, "EQ_OPER")
                    .IsUnique();

                entity.HasIndex(e => new { e.OperName, e.EqEquipNo }, "EQ_OPER_NAME")
                    .IsUnique();

                entity.HasIndex(e => new { e.DeptPmNotifyDept, e.EqEquipNo }, "EQ_PM_NOTIFY_DEPT")
                    .IsUnique();

                entity.HasIndex(e => new { e.PooltypVehType, e.LocReservLoc }, "EQ_POOL");

                entity.HasIndex(e => e.RefEquipPos, "EQ_REF_EQUIP");

                entity.HasIndex(e => new { e.SerialNo, e.EqEquipNo }, "EQ_SERIAL")
                    .IsUnique();

                entity.HasIndex(e => new { e.EqtypEquipType, e.EqEquipNo }, "EQ_TYPE")
                    .IsUnique();

                entity.Property(e => e.AcatCategoryNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ACAT_category_no");

                entity.Property(e => e.AcctAcctCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ACCT_acct_code");

                entity.Property(e => e.AcctRevAcctCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ACCT_rev_acct_code");

                entity.Property(e => e.ActualCoFirstOper)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("actual_co_first_oper")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ActualCompanyCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("actual_company_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AnyFuelType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("any_fuel_type")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.ApprovalLevel)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("approval_level")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AssetNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("asset_no");

                entity.Property(e => e.AssetType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("asset_type");

                entity.Property(e => e.AssocEquipNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("assoc_equip_no");

                entity.Property(e => e.AuthorizationNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("authorization_no");

                entity.Property(e => e.BaseMrpCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("base_mrp_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BaseMrpFirstOper)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("base_mrp_first_oper")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BillingCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("billing_code")
                    .IsFixedLength();

                entity.Property(e => e.BuyBack)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("buy_back")
                    .IsFixedLength();

                entity.Property(e => e.CapitalCostPosted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("capital_cost_posted")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.CapitalizedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("capitalized_date");

                entity.Property(e => e.CapitalizedValue)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("capitalized_value")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClassClassBench)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("class_class_bench");

                entity.Property(e => e.ClassClassMaint)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_maint");

                entity.Property(e => e.ClassClassMeter)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_meter");

                entity.Property(e => e.ClassClassPm)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_pm");

                entity.Property(e => e.ClassClassRental)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_rental");

                entity.Property(e => e.ClassClassShopSch)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_shop_sch");

                entity.Property(e => e.ClassClassStds)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLASS_class_stds");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.CommentArea)
                    .HasMaxLength(1200)
                    .IsUnicode(false)
                    .HasColumnName("comment_area");

                entity.Property(e => e.CommentAreaMsg)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasColumnName("comment_area_msg");

                entity.Property(e => e.CostCenter)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("cost_center");

                entity.Property(e => e.CostRptExclSwitch)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("cost_rpt_excl_switch")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.CpyCompanyCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CPY_company_code");

                entity.Property(e => e.CurOperIsFirst)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("cur_oper_is_first")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.CurrencyRowId)
                    .HasColumnName("currency_row_id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateAdded)
                    .HasColumnType("datetime")
                    .HasColumnName("date_added")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DbAcc).HasColumnName("DB_ACC");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("delivery_date");

                entity.Property(e => e.DeprCurDeclineBal)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("depr_cur_decline_bal")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DeprMthsRemaining)
                    .HasColumnName("depr_mths_remaining")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DeprecMonthsLife)
                    .HasColumnName("deprec_months_life")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DepreciationMethod)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("depreciation_method");

                entity.Property(e => e.DeptDeptCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_dept_code");

                entity.Property(e => e.DeptPmNotifyDept)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_pm_notify_dept");

                entity.Property(e => e.DeptTempLoanedTo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_temp_loaned_to");

                entity.Property(e => e.Description)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DielectricTesting)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("dielectric_testing")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.DisposalAuthority)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("disposal_authority");

                entity.Property(e => e.DisposalComment)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("disposal_comment");

                entity.Property(e => e.DisposalMethod)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("disposal_method");

                entity.Property(e => e.DisposalReason)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("disposal_reason");

                entity.Property(e => e.DutyCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("duty_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EqEquipNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EQ_equip_no");

                entity.Property(e => e.EqEquipNoNew)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EQ_equip_no_new");

                entity.Property(e => e.EqInvalidated)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("eq_invalidated")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.EqReplacedByUnit)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EQ_replaced_by_unit");

                entity.Property(e => e.EqReplacingUnit)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EQ_replacing_unit");

                entity.Property(e => e.EqcatEquipCategory)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EQCAT_equip_category");

                entity.Property(e => e.EqtypEquipType)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EQTYP_equip_type");

                entity.Property(e => e.EstMeterAtReplace)
                    .HasColumnName("est_meter_at_replace")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EstReplaceCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("est_replace_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EstReplaceMo).HasColumnName("est_replace_mo");

                entity.Property(e => e.EstReplaceYr).HasColumnName("est_replace_yr");

                entity.Property(e => e.ExceptionSwitches)
                    .HasMaxLength(104)
                    .IsUnicode(false)
                    .HasColumnName("exception_switches");

                entity.Property(e => e.ExcpRptExclSwitch)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("excp_rpt_excl_switch")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.FileDescription)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("file_description");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("file_path");

                entity.Property(e => e.FixedCostOther1)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_cost_other_1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedCostOther2)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_cost_other_2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedCostOther3)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_cost_other_3")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedInsuranceCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_insurance_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedLicensingCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_licensing_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedMonthlyCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_monthly_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FixedReplaceCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("fixed_replace_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FltFleetNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("FLT_fleet_no");

                entity.Property(e => e.FuelCardNo).HasColumnName("fuel_card_no");

                entity.Property(e => e.FuelTicketsOk)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("fuel_tickets_ok")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.GreenDiskNo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("green_disk_no");

                entity.Property(e => e.HasTachograph)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("has_tachograph")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.InServiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("in_service_date");

                entity.Property(e => e.InsInsuranceCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("INS_insurance_code");

                entity.Property(e => e.InspectionMonth)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("inspection_month");

                entity.Property(e => e.InvListExclSwitch)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("inv_list_excl_switch")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.IsAsset)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("is_asset")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.IsTestEquip)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("is_test_equip")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.LastFuelDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_fuel_date");

                entity.Property(e => e.LastMeter1Date)
                    .HasColumnType("datetime")
                    .HasColumnName("last_meter_1_date");

                entity.Property(e => e.LastMeter1Reading)
                    .HasColumnName("last_meter_1_reading")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastMeter2Date)
                    .HasColumnType("datetime")
                    .HasColumnName("last_meter_2_date");

                entity.Property(e => e.LastMeter2Reading)
                    .HasColumnName("last_meter_2_reading")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastMeterSource)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("last_meter_source");

                entity.Property(e => e.LastOilChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_oil_change_date");

                entity.Property(e => e.LastOilChangeMtr1)
                    .HasColumnName("last_oil_change_mtr1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastOilChangeMtr2)
                    .HasColumnName("last_oil_change_mtr2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPmMeter1)
                    .HasColumnName("last_pm_meter_1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPmMeter2)
                    .HasColumnName("last_pm_meter_2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPmSchedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_pm_sched_date");

                entity.Property(e => e.LastPmSlot)
                    .HasColumnName("last_pm_slot")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPmStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_pm_start_date");

                entity.Property(e => e.LastPmStartMeter1)
                    .HasColumnName("last_pm_start_meter_1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastPmStartMeter2)
                    .HasColumnName("last_pm_start_meter_2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastRegularInspDt)
                    .HasColumnType("datetime")
                    .HasColumnName("last_regular_insp_dt");

                entity.Property(e => e.LastReservDateIn)
                    .HasColumnType("datetime")
                    .HasColumnName("last_reserv_date_in");

                entity.Property(e => e.LastReservDateOut)
                    .HasColumnType("datetime")
                    .HasColumnName("last_reserv_date_out");

                entity.Property(e => e.LastReservMeterIn)
                    .HasColumnName("last_reserv_meter_in")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastReservMtrOut)
                    .HasColumnName("last_reserv_mtr_out")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastStatuteInspDt)
                    .HasColumnType("datetime")
                    .HasColumnName("last_statute_insp_dt");

                entity.Property(e => e.LastYardckDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("last_yardck_datetime");

                entity.Property(e => e.LeaseExpirationDt)
                    .HasColumnType("datetime")
                    .HasColumnName("lease_expiration_dt");

                entity.Property(e => e.LeaseLeaseId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LEASE_lease_id");

                entity.Property(e => e.LeaseResidualValue)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("lease_residual_value")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LicPlatingNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lic_plating_no");

                entity.Property(e => e.LicenseNo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("license_no");

                entity.Property(e => e.LicenseNo2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("license_no_2");

                entity.Property(e => e.LicenseSt)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("license_st");

                entity.Property(e => e.LifeTotalMeter1)
                    .HasColumnName("life_total_meter_1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LifeTotalMeter2)
                    .HasColumnName("life_total_meter_2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LocAccessRtsLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_access_rts_loc");

                entity.Property(e => e.LocAssgnMobileLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_assgn_mobile_loc");

                entity.Property(e => e.LocAssignPmLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_assign_pm_loc");

                entity.Property(e => e.LocAssignReprLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_assign_repr_loc");

                entity.Property(e => e.LocCurrentLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_current_loc");

                entity.Property(e => e.LocLastFuelLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_last_fuel_loc");

                entity.Property(e => e.LocReservLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_reserv_loc");

                entity.Property(e => e.LocSlaLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_sla_loc");

                entity.Property(e => e.LocStationLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_station_loc");

                entity.Property(e => e.LocStoredLoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOC_stored_loc");

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("manufacturer");

                entity.Property(e => e.MaxMeter1Value)
                    .HasColumnName("max_meter_1_value")
                    .HasDefaultValueSql("((999999))");

                entity.Property(e => e.MaxMeter2Value)
                    .HasColumnName("max_meter_2_value")
                    .HasDefaultValueSql("((999999))");

                entity.Property(e => e.Meter1AtDelivery)
                    .HasColumnName("meter_1_at_delivery")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter1PrevTotal)
                    .HasColumnName("meter_1_prev_total")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter1Type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("meter_1_type");

                entity.Property(e => e.Meter2AtDelivery)
                    .HasColumnName("meter_2_at_delivery")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter2PrevTotal)
                    .HasColumnName("meter_2_prev_total")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter2Type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("meter_2_type");

                entity.Property(e => e.MeterPostingFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("meter_posting_flag")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Model)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("model");

                entity.Property(e => e.MonthlyRent)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("monthly_rent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MonthsInOperation)
                    .HasColumnName("months_in_operation")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NextPmSchedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("next_pm_sched_date");

                entity.Property(e => e.NextPmSlot)
                    .HasColumnName("next_pm_slot")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OLicenseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("o_license_date");

                entity.Property(e => e.OffRoadPct)
                    .HasColumnName("off_road_pct")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OffRoadUse)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("off_road_use")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.OilType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("oil_type")
                    .IsFixedLength();

                entity.Property(e => e.OnTempLoan)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("on_temp_loan")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.OperName)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("oper_name");

                entity.Property(e => e.OperOperNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OPER_oper_no");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrigRegistDate)
                    .HasColumnType("datetime")
                    .HasColumnName("orig_regist_date");

                entity.Property(e => e.OriginalCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("original_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OutfittingCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("outfitting_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OutfittingEffort)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("outfitting_effort")
                    .IsFixedLength();

                entity.Property(e => e.OwnLeaseCustomer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("own_lease_customer");

                entity.Property(e => e.ParentLifeMeter1)
                    .HasColumnName("parent_life_meter_1")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ParkingStall)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("parking_stall");

                entity.Property(e => e.PlannedDelivDate)
                    .HasColumnType("datetime")
                    .HasColumnName("planned_deliv_date");

                entity.Property(e => e.PlannedInsvcDate)
                    .HasColumnType("datetime")
                    .HasColumnName("planned_insvc_date");

                entity.Property(e => e.PlannedLicplDate)
                    .HasColumnType("datetime")
                    .HasColumnName("planned_licpl_date");

                entity.Property(e => e.PlannedRetirDate)
                    .HasColumnType("datetime")
                    .HasColumnName("planned_retir_date");

                entity.Property(e => e.PmFuelOverride)
                    .HasColumnName("pm_fuel_override")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PmFuelSinceLast)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("pm_fuel_since_last")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PmMeter1Interval)
                    .HasColumnName("pm_meter_1_interval")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PmMeter2Interval)
                    .HasColumnName("pm_meter_2_interval")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PmPrefShift)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("pm_pref_shift");

                entity.Property(e => e.PmProgType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("pm_prog_type");

                entity.Property(e => e.PooltypVehType)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("POOLTYP_veh_type");

                entity.Property(e => e.PriShopPriority)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PRI_shop_priority");

                entity.Property(e => e.ProcstProcStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PROCST_proc_status");

                entity.Property(e => e.QtyOpenWorkOrders)
                    .HasColumnName("qty_open_work_orders")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.QtyTires)
                    .HasColumnName("qty_tires")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RadioBuildingLoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("radio_building_loc");

                entity.Property(e => e.RadioNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("radio_no");

                entity.Property(e => e.RadioOtherLoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("radio_other_loc");

                entity.Property(e => e.RadioVehicleLoc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("radio_vehicle_loc");

                entity.Property(e => e.ReadyDisposition)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ready_disposition")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.RefEquipPos)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ref_equip_pos");

                entity.Property(e => e.RegistExpireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("regist_expire_date");

                entity.Property(e => e.ReplCostComputed)
                    .HasColumnName("repl_cost_computed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReplCostRecovYear)
                    .HasColumnName("repl_cost_recov_year")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReplCstTotrcovLif)
                    .HasColumnName("repl_cst_totrcov_lif")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReplCstTotrcovYr)
                    .HasColumnName("repl_cst_totrcov_yr")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReplaceCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("replace_code")
                    .IsFixedLength();

                entity.Property(e => e.ReservStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("reserv_status");

                entity.Property(e => e.ResvLastReservNo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RESV_last_reserv_no");

                entity.Property(e => e.RetireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("retire_date");

                entity.Property(e => e.RowId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("row_id");

                entity.Property(e => e.SaleDate)
                    .HasColumnType("datetime")
                    .HasColumnName("sale_date");

                entity.Property(e => e.SalePrice)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("sale_price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalvageValue)
                    .HasColumnName("salvage_value")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalvageValuePct)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("salvage_value_pct")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SerialNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("serial_no");

                entity.Property(e => e.ShippingCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("shipping_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SlaStatus)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sla_status");

                entity.Property(e => e.StatInspectInterv).HasColumnName("stat_inspect_interv");

                entity.Property(e => e.StatInspectMonth).HasColumnName("stat_inspect_month");

                entity.Property(e => e.StatInspectYear).HasColumnName("stat_inspect_year");

                entity.Property(e => e.StatusCodes)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("status_codes");

                entity.Property(e => e.StudyCode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("study_code");

                entity.Property(e => e.SysgrSystemGrouping)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SYSGR_system_grouping");

                entity.Property(e => e.TaxTaxCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TAX_tax_code");

                entity.Property(e => e.TempLoanDate)
                    .HasColumnType("datetime")
                    .HasColumnName("temp_loan_date");

                entity.Property(e => e.TireType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("tire_type");

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.TrackParentMeter)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("track_parent_meter")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.TransfereeAddress1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("transferee_address1");

                entity.Property(e => e.TransfereeAddress2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("transferee_address2");

                entity.Property(e => e.TransfereeAddress3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("transferee_address3");

                entity.Property(e => e.TransfereeAddress4)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("transferee_address4");

                entity.Property(e => e.TransfereeName)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("transferee_name");

                entity.Property(e => e.UsageTicketFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("usage_ticket_flag")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.UsageTicketsOk)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("usage_tickets_ok")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.UserStatus1)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("user_status_1");

                entity.Property(e => e.UserStatus2)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("user_status_2");

                entity.Property(e => e.UserStatus3)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("user_status_3");

                entity.Property(e => e.VatCost)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("vat_cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WorkOrderStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("work_order_status");

                entity.Property(e => e.WorkOrdersOk)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("work_orders_ok")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.XDatetimeInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("X_datetime_insert")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.XUseridInsert)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("X_userid_insert")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.YardckAvailRepair)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("yardck_avail_repair")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.Year).HasColumnName("year");
            });
        }
    }
}
