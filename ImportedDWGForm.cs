using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamoBundle
{
    public partial class ImportedDWGForm : Form
    {
        Autodesk.Revit.UI.UIDocument uidoc;
        private Autodesk.Revit.DB.Document doc;
        private List<ImportedDWG> dwgList;
        
        internal ImportedDWGForm()
        {
            InitializeComponent();
        }

        internal ImportedDWGForm(Autodesk.Revit.UI.UIDocument uidoc, List<ImportedDWG> dwg)
        {
            InitializeComponent();

            this.uidoc = uidoc;
            this.doc = uidoc.Document;
            this.dwgList = dwg;

            InitializeList();
        }

        private void InitializeList()
        {
            foreach(ImportedDWG dwg in dwgList)
            {
                impDataGridView.Rows.Add(dwg.name, dwg.uniqueId);
            }
        }

        private void impDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(impDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                ImportedDWG dwg = dwgList.First(x => x.uniqueId.Equals(impDataGridView.Rows[e.RowIndex].Cells[1].Value));

                if(dwg.view != null)
                {
                    uidoc.ActiveView = dwg.view;
                    uidoc.Selection.SetElementIds(new List<Autodesk.Revit.DB.ElementId>() { dwg.id });
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This dwg is shown in all views.");
                }
            }
        }

        private void deleteSelectedbtn_Click(object sender, EventArgs e)
        {
            if (impDataGridView.SelectedRows.Count > 0)
            {
                var selected = impDataGridView.SelectedRows
                    .OfType<DataGridViewRow>()
                    .Select(x => x.Cells[1].Value)
                    .ToList();

                var query = dwgList
                    .Where(x => selected.Contains(x.uniqueId))
                    .Select(x => x.id)
                    .ToList();

                DeleteTransaction(query);
            }
            else if(impDataGridView.SelectedCells.Count > 0)
            {
                var selected = impDataGridView.SelectedCells
                    .OfType<DataGridViewCell>()
                    .Select(x => impDataGridView.Rows[x.RowIndex].Cells[1].Value)
                    .ToList();

                var query = dwgList
                    .Where(x => selected.Contains(x.uniqueId))
                    .Select(x => x.id)
                    .ToList();

                DeleteTransaction(query);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void deleteAllbtn_Click(object sender, EventArgs e)
        {
            DeleteTransaction(dwgList.Select(x => x.id).ToList());

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DeleteTransaction(List<Autodesk.Revit.DB.ElementId> list)
        {
            using (Autodesk.Revit.DB.Transaction t = new Autodesk.Revit.DB.Transaction(doc, "Delete Imported DWGs"))
            {
                t.Start();
                doc.Delete(list);
                t.Commit();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
