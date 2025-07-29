namespace ISC_Project.Interface
{
    public interface IExportStudentService
    {
        Task<(byte[] fileContents, string fileName)> ExportStudentsToExcelAsync(int classId);
    }
}
