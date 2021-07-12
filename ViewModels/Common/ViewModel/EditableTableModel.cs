using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ViewModels.Common.ViewModel
{
    public class EditableTableModel<T> : Notifier where T : Notifier
    {
        private ObservableCollection<T> _tableContent = new();

        public ObservableCollection<T> TableContent
        {
            get => _tableContent;
            set
            {
                _tableContent = value;
                if (value != null)
                {
                    SubscribeOnChanges();
                }
            }
        }

        public IList<T> Added { get; } = new List<T>();

        public IList<T> Removed { get; } = new List<T>();

        public IList<T> Updated { get; } = new List<T>();

        private void SubscribeOnChanges()
        {
            TableContent.CollectionChanged += ContentOnCollectionChanged;
            foreach (var item in TableContent)
            {
                item.PropertyChanged += (_, __) =>
                {
                    ContentOnPropertyChanged(item);
                };
                SubscribeOnPropertyChanged(item);
            }
        }

        private void ContentOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add && e.NewItems[0] is T certificateToAdd)
            {
                Added.Add(certificateToAdd);
            }

            if (e.Action is NotifyCollectionChangedAction.Remove && e.OldItems[0] is T certificateToRemove)
            {
                if (Added.Contains(certificateToRemove))
                {
                    Added.Remove(certificateToRemove);
                }
                else
                {
                    Removed.Add(certificateToRemove);
                }
            }
        }

        private void SubscribeOnPropertyChanged(T item)
        {
            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(item);
                if (propertyValue is Notifier notifier)
                {
                    notifier.PropertyChanged += (_, __) =>
                    {
                        ContentOnPropertyChanged(item);
                    };
                }
            }
        }

        private void ContentOnPropertyChanged(T item)
        {
            if (Updated.Contains(item))
            {
                return;
            }

            Updated.Add(item);
        }
    }
}