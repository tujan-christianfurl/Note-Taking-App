using System;
using System.Collections.ObjectModel;
using System.Linq;
using ToDoClassNotes;

namespace Note_Taking_App;

public partial class MainPage : ContentPage
{
    ObservableCollection<ToDoClass> todoList = new ObservableCollection<ToDoClass>();

    int selectedId = -1;
    int autoIncrementId = 1;

    public MainPage()
    {
        InitializeComponent();
        todoLV.ItemsSource = todoList;
    }

    private async void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text) ||
            string.IsNullOrWhiteSpace(detailsEditor.Text))
        {
            await DisplayAlert("Error", "Please enter title and details.", "OK");
            return;
        }

        todoList.Add(new ToDoClass
        {
            id = autoIncrementId++,
            title = titleEntry.Text,
            detail = detailsEditor.Text
        });

        ClearFields();
    }

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int id = Convert.ToInt32(btn.ClassId);

        var itemToDelete = todoList.FirstOrDefault(x => x.id == id);

        if (itemToDelete != null)
        {
            todoList.Remove(itemToDelete);
        }
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selectedItem = (ToDoClass)e.SelectedItem;

        selectedId = selectedItem.id;

        titleEntry.Text = selectedItem.title;
        detailsEditor.Text = selectedItem.detail;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void todoLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        todoLV.SelectedItem = e.Item;
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        if (selectedId == -1)
            return;

        var itemToEdit = todoList.FirstOrDefault(x => x.id == selectedId);

        if (itemToEdit != null)
        {
            itemToEdit.title = titleEntry.Text;
            itemToEdit.detail = detailsEditor.Text;
        }

        CancelEdit(null, null);
    }

    private void CancelEdit(object sender, EventArgs e)
    {
        selectedId = -1;
        todoLV.SelectedItem = null;

        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;

        ClearFields();
    }

    private void ClearFields()
    {
        titleEntry.Text = string.Empty;
        detailsEditor.Text = string.Empty;
    }
}