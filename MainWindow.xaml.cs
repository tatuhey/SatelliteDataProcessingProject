using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Galileo6;


namespace SatelliteDataProcessingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Others
        private void ButtonsDisable_A()
        {
            btnBinaryIterativeA.IsEnabled = false;
            btnBinaryRecursiveA.IsEnabled = false;
        }

        private void ButtonsDisable_B()
        {
            btnBinaryIterativeB.IsEnabled = false;
            btnBinaryRecursiveB.IsEnabled = false;
        }

        private void ButtonsEnable_A()
        {
            btnBinaryIterativeA.IsEnabled = true;
            btnBinaryRecursiveA.IsEnabled = true;
        }

        private void ButtonsEnable_B()
        {
            btnBinaryIterativeB.IsEnabled = true;
            btnBinaryRecursiveB.IsEnabled = true;
        }

        private void ClearTexboxes()
        {
            tbSelTicksA.Text = string.Empty;
            tbInsTicksA.Text = string.Empty;
            tbSelTicksB.Text = string.Empty;
            tbInsTicksB.Text = string.Empty;
            tbTargetA.Text = string.Empty;
            tbTargetB.Text = string.Empty;
            tbBinaryIterativeA.Text = string.Empty;
            tbBinaryIterativeB.Text = string.Empty;
            tbBinaryRecursiveA.Text = string.Empty;
            tbBinaryRecursiveB.Text = string.Empty;
            lbSensorA1.Items.Clear();
            lbSensorB1.Items.Clear();
        }
        #endregion

        #region Typing Prevention
        private void tbSelTicksA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbInsTicksA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbBinaryIterativeA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbBinaryRecursiveA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbSelTicksB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbInsTicksB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbBinaryIterativeB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbBinaryRecursiveB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void intUpDoSigma_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Suppress the input
            }
        }

        private void intUpDoMu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Suppress the input
            }
        }
        #endregion

        #region Global Methods
        //4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic namespace.
        //      The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data structures (array, list, etc)
        //      in the implementation of this application. The two LinkedLists of type double are to be declared as global within the “public partial class”.

        LinkedList<double> sensorAdata = new LinkedList<double>();
        LinkedList<double> sensorBdata = new LinkedList<double>();


        //4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
        //      Create a method called “LoadData” which will populate both LinkedLists.Declare an instance of the Galileo library in the method
        //      and create the appropriate loop construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList,
        //      while the data from Sensor B will populate the second LinkedList.The LinkedList size will be hardcoded inside the method and must be equal to 400.
        //      The input parameters are empty, and the return type is void.
        private void LoadData()
        {
            // Create an instance of the Galileo library
            Galileo6.ReadData galileoInstance = new Galileo6.ReadData();

            // Populate LinkedLists
            int dataSize = 400; // LinkedList size
            for (int i = 0; i < dataSize; i++)
            {
                double sensorAValue = galileoInstance.SensorA(Convert.ToDouble(intUpDoMu.Text), Convert.ToDouble(intUpDoSigma.Text));
                double sensorBValue = galileoInstance.SensorB(Convert.ToDouble(intUpDoMu.Text), Convert.ToDouble(intUpDoSigma.Text));

                sensorAdata.AddLast(sensorAValue);
                sensorBdata.AddLast(sensorBValue);
            }
        }

        //4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        //      Add column titles “Sensor A” and “Sensor B” to the ListView.The input parameters are empty, and the return type is void.
        private void ShowAllSensorData()
        {
            // Add data to ListView
            lvSensors.Items.Clear();
            var nodeA = sensorAdata.First;
            var nodeB = sensorBdata.First;

            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Sensor A",
                DisplayMemberBinding = new System.Windows.Data.Binding("SensorAValue")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Sensor B",
                DisplayMemberBinding = new System.Windows.Data.Binding("SensorBValue")
            });

            // Assign columns to ListView
            lvSensors.View = gridView;

            while (nodeA != null && nodeB != null)
            {
                lvSensors.Items.Add(new { SensorAValue = nodeA.Value, SensorBValue = nodeB.Value });
                nodeA = nodeA.Next;
                nodeB = nodeB.Next;
            }
        }


        //4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.The input parameters are empty, and the return type is void.
        private void btnSensors_Click(object sender, RoutedEventArgs e)
        {
            ClearTexboxes();
            lvSensors.Items.Clear();
            sensorAdata.Clear();
            sensorBdata.Clear();
            LoadData();
            ShowAllSensorData();
            ButtonsDisable_A();
            ButtonsDisable_B();
        }
        #endregion

        #region Utility Methods
        //4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        //      The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
        private int NumberOfNodes<T>(LinkedList<T> list)
        {
            int count = 0;
            var currentNode = list.First;

            while (currentNode != null)
            {
                count++;
                currentNode = currentNode.Next;
            }

            return count;
        }

        //4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
        //      The method signature will have two input parameters; a LinkedList, and the ListBox name.The calling code argument is the linkedlist name and the listbox name.
        private void DisplayListboxData<T>(LinkedList<T> list, ListBox listBox)
        {
            listBox.Items.Clear();

            foreach (var item in list)
            {
                listBox.Items.Add(item);
            }
        }
        #endregion

        #region Sort and Search Methods
        //4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList,
        //      while the calling code argument is the linkedlist name.The method code must follow the pseudo code supplied below in the Appendix.The return type is Boolean.
        private bool SelectionSort<T>(LinkedList<T> list)
        {
            if (list == null || list.Count <= 1)
                return false; // Nothing to sort

            int min, max;
            max = list.Count;
            for (int i = 0; i < max - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if (Comparer<T>.Default.Compare(list.ElementAt(j), list.ElementAt(min)) < 0)
                        min = j;
                }

                // Find nodes and swap values
                LinkedListNode<T> currentMin = list.Find(list.ElementAt(min));
                LinkedListNode<T> currentI = list.Find(list.ElementAt(i));

                T temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }

            return true;
        }



        //4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList,
        //      while the calling code argument is the linkedlist name.The method code must follow the pseudo code supplied below in the Appendix. The return type is Boolean.
        private bool InsertionSort<T>(LinkedList<T> list)
        {
            if (list == null || list.Count <= 1)
                return false; // Nothing to sort

            int max = list.Count;

            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    var currentNode = list.Find(list.ElementAt(j));
                    var previousNode = list.Find(list.ElementAt(j - 1));

                    var comparer = Comparer<T>.Default;
                    if (comparer.Compare(previousNode.Value, currentNode.Value) > 0)
                    {
                        // Swap values
                        T temp = previousNode.Value;
                        previousNode.Value = currentNode.Value;
                        currentNode.Value = temp;
                    }
                }
            }

            return true;
        }



        //4.9	Create a method called “BinarySearchIterative” which has the following four parameters:
        //      LinkedList, SearchValue, Minimum and Maximum.This method will return an integer of the linkedlist element from a successful search
        //      or the nearest neighbour value.The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //      The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchIterative(LinkedList<double> linkedList, double searchValue, int minIndex, int maxIndex)
        {
            while (minIndex <= maxIndex)
            {
                int middleIndex = (minIndex + maxIndex) / 2;

                if (linkedList.ElementAt(middleIndex) == searchValue)
                {
                    return middleIndex;
                }
                else if (linkedList.ElementAt(middleIndex) < searchValue)
                {
                    minIndex = middleIndex + 1;
                }
                else
                {
                    maxIndex = middleIndex - 1;
                }
            }

            // If the search value is not found, return the nearest neighbor value
            if (maxIndex < 0)
            {
                return minIndex;
            }
            else if (minIndex >= linkedList.Count)
            {
                return maxIndex;
            }
            else
            {
                double minValue = linkedList.ElementAt(maxIndex);
                double maxValue = linkedList.ElementAt(minIndex);

                if (Math.Abs(minValue - searchValue) <= Math.Abs(maxValue - searchValue))
                {
                    return maxIndex;
                }
                else
                {
                    return minIndex;
                }
            }
        }

        //4.10	Create a method called “BinarySearchRecursive” which has the following four parameters:
        //      LinkedList, SearchValue, Minimum and Maximum.This method will return an integer of the linkedlist element
        //      from a successful search or the nearest neighbour value.The calling code argument is the linkedlist name, search value, minimum list size
        //      and the number of nodes in the list. The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchRecursive(LinkedList<double> linkedList, double searchValue, int minIndex, int maxIndex)
        {
            if (minIndex <= maxIndex)
            {
                int middleIndex = (minIndex + maxIndex) / 2;

                if (linkedList.ElementAt(middleIndex) == searchValue)
                {
                    return middleIndex;
                }
                else if (linkedList.ElementAt(middleIndex) < searchValue)
                {
                    return BinarySearchRecursive(linkedList, searchValue, middleIndex + 1, maxIndex);
                }
                else
                {
                    return BinarySearchRecursive(linkedList, searchValue, minIndex, middleIndex - 1);
                }
            }
            else
            {
                // If the search value is not found, return the nearest neighbor value
                if (maxIndex < 0)
                {
                    return minIndex;
                }
                else if (minIndex >= linkedList.Count)
                {
                    return maxIndex;
                }
                else
                {
                    double minValue = linkedList.ElementAt(maxIndex);
                    double maxValue = linkedList.ElementAt(minIndex);

                    if (Math.Abs(minValue - searchValue) <= Math.Abs(maxValue - searchValue))
                    {
                        return maxIndex;
                    }
                    else
                    {
                        return minIndex;
                    }
                }
            }
        }

        #endregion

        #region UI Button Methods
        //4.11	Create four button click methods that will search the LinkedList for an integer value entered into a textbox on the form. The four methods are:
        //      1.	Method for Sensor A and Binary Search Iterative
        //      2.	Method for Sensor A and Binary Search Recursive
        //      3.	Method for Sensor B and Binary Search Iterative
        //      4.	Method for Sensor B and Binary Search Recursive
        //      The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method.
        //      Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox.
        //      Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.

        private void btnBinaryIterativeA_Click(object sender, RoutedEventArgs e)
        {
            double searchValue;
            if (!double.TryParse(tbTargetA.Text, out searchValue))
            {
                MessageBox.Show("Invalid search value.");
                return;
            }

            if (SelectionSort(sensorAdata))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int resultIndex = BinarySearchIterative(sensorAdata, searchValue, 0, sensorAdata.Count - 1);
                stopwatch.Stop();

                tbBinaryIterativeA.Text = stopwatch.ElapsedTicks.ToString() + " ticks";
                if (resultIndex != -1)
                {
                    DisplayListboxData(sensorAdata, lbSensorA1);
                    HighlightSearchResults(resultIndex, lbSensorA1);
                }
                else
                {
                    MessageBox.Show("Value not found.");
                }
            }
            else
            {
                MessageBox.Show("Data needs to be sorted for binary search.");
            }
        }

        private void btnBinaryRecursiveA_Click(object sender, RoutedEventArgs e)
        {
            double searchValue;
            if (!double.TryParse(tbTargetA.Text, out searchValue))
            {
                MessageBox.Show("Invalid search value.");
                return;
            }

            if (SelectionSort(sensorAdata))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int resultIndex = BinarySearchRecursive(sensorAdata, searchValue, 0, sensorAdata.Count - 1);
                stopwatch.Stop();

                tbBinaryRecursiveA.Text = stopwatch.ElapsedTicks.ToString() + " ticks";

                if (resultIndex != -1)
                {
                    DisplayListboxData(sensorAdata, lbSensorA1);
                    HighlightSearchResults(resultIndex, lbSensorA1);
                }
                else
                {
                    MessageBox.Show("Value not found.");
                }
            }
            else
            {
                MessageBox.Show("Data needs to be sorted for binary search.");
            }
        }

        private void btnBinaryIterativeB_Click(object sender, RoutedEventArgs e)
        {
            double searchValue;
            if (!double.TryParse(tbTargetB.Text, out searchValue))
            {
                MessageBox.Show("Invalid search value.");
                return;
            }

            if (SelectionSort(sensorBdata))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int resultIndex = BinarySearchIterative(sensorBdata, searchValue, 0, sensorBdata.Count - 1);
                stopwatch.Stop();

                tbBinaryIterativeB.Text = stopwatch.ElapsedTicks.ToString() + " ticks";

                if (resultIndex != -1)
                {
                    DisplayListboxData(sensorBdata, lbSensorB1);
                    HighlightSearchResults(resultIndex, lbSensorB1);
                }
                else
                {
                    MessageBox.Show("Value not found.");
                }
            }
            else
            {
                MessageBox.Show("Data needs to be sorted for binary search.");
            }
        }

        private void btnBinaryRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            double searchValue;
            if (!double.TryParse(tbTargetB.Text, out searchValue))
            {
                MessageBox.Show("Invalid search value.");
                return;
            }

            if (SelectionSort(sensorBdata))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int resultIndex = BinarySearchRecursive(sensorBdata, searchValue, 0, sensorBdata.Count - 1);
                stopwatch.Stop();

                tbBinaryRecursiveB.Text = stopwatch.ElapsedTicks.ToString() + " ticks";

                if (resultIndex != -1)
                {
                    DisplayListboxData(sensorBdata, lbSensorB1);
                    HighlightSearchResults(resultIndex, lbSensorB1);
                }
                else
                {
                    MessageBox.Show("Value not found.");
                }
            }
            else
            {
                MessageBox.Show("Data needs to be sorted for binary search.");
            }
        }

        private void HighlightSearchResults(int resultIndex, ListBox listbox)
        {
            listbox.UnselectAll();
            listbox.Focus();

            int totalNodes = NumberOfNodes(sensorAdata);

            int start = Math.Max(resultIndex - 2, 0);
            int end = Math.Min(resultIndex + 2, totalNodes - 1);

            for (int i = start; i <= end; i++)
            {
                listbox.SelectedItems.Add(listbox.Items[i]);
            }
        }

        //4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.The four methods are:
        //      1.	Method for Sensor A and Selection Sort
        //      2.	Method for Sensor A and Insertion Sort
        //      3.	Method for Sensor B and Selection Sort
        //      4.	Method for Sensor B and Insertion Sort
        //      The button method must start a stopwatch before calling the sort method.
        //      Once the sort is complete the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox.
        //      Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the appropriate sensor.

        private void btnSelSortA_Click(object sender, RoutedEventArgs e)
        {
            ButtonsEnable_A();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            SelectionSort(sensorAdata);

            stopwatch.Stop();
            tbSelTicksA.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            DisplayListboxData(sensorAdata, lbSensorA1);
        }

        private void btnInsSortA_Click(object sender, RoutedEventArgs e)
        {
            ButtonsEnable_A();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            InsertionSort(sensorAdata);

            stopwatch.Stop();
            tbInsTicksA.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            DisplayListboxData(sensorAdata, lbSensorA1);
        }

        private void btnSelSortB_Click(object sender, RoutedEventArgs e)
        {
            ButtonsEnable_B();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            SelectionSort(sensorBdata);

            stopwatch.Stop();
            tbSelTicksB.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            DisplayListboxData(sensorBdata, lbSensorB1);
        }

        private void btnInsSortB_Click(object sender, RoutedEventArgs e)
        {
            ButtonsEnable_B();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            InsertionSort(sensorBdata);

            stopwatch.Stop();
            tbInsTicksB.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            DisplayListboxData(sensorBdata, lbSensorB1);
        }


        //4.13	Add two numeric input controls for Sigma and Mu.The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
        //      Set the default value to 10. The value for Mu must be limited with a minimum of 35 and a maximum of 75. Set the default value to 50.


        //4.14	Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.

        private void tbTargetA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Suppress the input
            }
        }

        private void tbTargetB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Suppress the input
            }
        }




        #endregion


        //4.15	All code is required to be adequately commented.Map the programming criteria and features to your code/methods
        //      by adding comments/regions above the method signatures.Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).


    }
}