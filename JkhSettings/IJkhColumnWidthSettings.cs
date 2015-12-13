namespace JkhSettings
{
	public interface IJkhColumnWidthSettings
	{
		string ColumnWidthsToString();
		bool RestoreColumnWidths(int[] widths);
	}

	public delegate string CustomSettingsXmlSaveColumnWidths();
	public delegate bool CustomSettingsXmlRestoreColumnWidths(int[] widths);
}