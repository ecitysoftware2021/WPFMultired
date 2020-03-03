using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.Classes
{
    public enum ELogType
    {
        General = 0,
        Error = 1,
        Device = 2
    }

    public enum EModalType
    {
        Cancell = 0,
        NotExistAccount = 1,
        Error = 2,
        MaxAmount = 3,
        Information = 4,
        Preload = 5,
        NoPaper = 6
    }

    public enum EError
    {
        Printer = 1,
        Nopapper = 2,
        Device = 3,
        Aplication = 5,
        Api = 6,
        Customer = 7,
        Internet = 8
    }

    public enum ELevelError
    {
        Mild = 3,
        Medium = 2,
        Strong = 1,
    }

    public enum UserControlView
    {
        Main,
        Consult,
        Menu,
        MenuCompaniesUserControl,
        PaySuccess,
        Pay,
        Payer,
        ReturnMony,
        Login,
        Config,
        Admin,
        Certificates,
        PrintFile,
        DataList
    }

    public enum ETransactionState
    {
        Initial = 1,
        Success = 2,
        CancelError = 6,
        Cancel = 3,
        Error = 5,
        ErrorService = 4
    }

    public enum ETransactionType
    {
        Withdrawal = 15,
        Pay = 19
    }

    public enum EProcedureType
    {
        Withdrawal = 3,
        Pay = 1
    }

    public enum ETypeDevice
    {
        AP = 2,
        DP = 3,
        MD = 8,
        MA = 4
    }

    public enum ETypeAdministrator
    {
        Balancing = 3,
        Upload = 1,
        Diminish = 2,
        ReUploat = 4,
    }

    public enum ETypePayer
    {
        Person = 1,
        Establishment = 2
    }

    public enum ETramite
    {
        DepositoEfectivo = 13,
        RetiroEfectivo = 10
    }

    public enum ETypeService
    {
        Type_Transaction = 0,
        Institutions = 1,
        Type_Document = 2,
        Products_Client = 3,
        Generate_OTP = 4,
        Report_Transaction = 5,
        Report_Transaction_BD = 12,
        Validate_OTP = 6,
        Idioms = 7,
        Consult_QR = 10,
        Validate_Status_Admin = 11,
        Operation_Admin = 13,
        Procces_Admin = 14,
        Validate_Admin_QR = 15
    }

    public enum ETypeDetailModel
    {
        Payment = 3,
        Withdrawal = 1,
        CodeOTP = 2,
        Qr = 4,
    }
}
