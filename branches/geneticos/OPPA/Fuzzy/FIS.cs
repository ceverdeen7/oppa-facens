using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Fuzzy;

namespace OPPA.Fuzzy
{
    public class FIS
    {
        #region Functions

        #region WheelAngle
        private static TrapezoidalFunction tfVeryNegative = new TrapezoidalFunction(-40, -25, TrapezoidalFunction.EdgeType.Right),
            tfNegative = new TrapezoidalFunction(-40, -25, -10),
            tfLittleNegative = new TrapezoidalFunction(-25, -10, 0f),
            tfZero = new TrapezoidalFunction(-10, 0f, 10),
            tfLittlePositive = new TrapezoidalFunction(0f, 10, 25),
            tfPositive = new TrapezoidalFunction(10, 25, 40),
            tfVeryPositive = new TrapezoidalFunction(25, 40, TrapezoidalFunction.EdgeType.Left);
        #endregion

        #region SteeringWheel
        private static TrapezoidalFunction tfLeftMax = new TrapezoidalFunction(-1.875f, -1.25f, TrapezoidalFunction.EdgeType.Right),
            tfLeftMedium = new TrapezoidalFunction(-1.875f, -1.25f, -0.625f),
            tfLeftMin = new TrapezoidalFunction(-1.25f, -0.625f, 0f),
            tfMiddle = new TrapezoidalFunction(-0.625f, 0f, 0.625f),
            tfRightMin = new TrapezoidalFunction(0f, 0.625f, 1.25f),
            tfRightMedium = new TrapezoidalFunction(0.625f, 1.25f, 1.875f),
            tfRightMax = new TrapezoidalFunction(1.25f, 1.875f, TrapezoidalFunction.EdgeType.Left);
        #endregion

        #region Friction
        private static TrapezoidalFunction tfNullFriction = new TrapezoidalFunction(0, 2, TrapezoidalFunction.EdgeType.Right),
            tfLowFriction = new TrapezoidalFunction(0, 2, 4),
            tfMediumFriction = new TrapezoidalFunction(2, 4, 6),
            tfHighFriction = new TrapezoidalFunction(4, 6, TrapezoidalFunction.EdgeType.Left);
        #endregion

        #region Speed
        private static TrapezoidalFunction tfStopped = new TrapezoidalFunction(2.5f, 10, TrapezoidalFunction.EdgeType.Right),
            tfVerySlow = new TrapezoidalFunction(2.5f, 10, 20),
            tfSlow = new TrapezoidalFunction(10, 20, 30, 40),
            tfRegular = new TrapezoidalFunction(30, 40, 60, 70),
            tfLittleFast = new TrapezoidalFunction(60, 70, 90, 100),
            tfFast = new TrapezoidalFunction(90, 100, 120, 130),
            tfVeryFast = new TrapezoidalFunction(120, 130, TrapezoidalFunction.EdgeType.Left);
        #endregion

        #region Accelerator/Brake
        private static TrapezoidalFunction tfNone = new TrapezoidalFunction(0.05f, 0.1f, TrapezoidalFunction.EdgeType.Right), 
            tfLittle = new TrapezoidalFunction(0.05f, 0.1f, 0.2f),
            tfMedium = new TrapezoidalFunction(0.1f, 0.2f, 0.7f, 0.9f),
            tfMuch = new TrapezoidalFunction(0.7f, 0.9f, TrapezoidalFunction.EdgeType.Left);
        #endregion

        #endregion

        #region FuzzySets

        #region WheelAngle
        private static FuzzySet fsVeryNegative = new FuzzySet("VeryNegative", tfVeryNegative),
            fsNegative = new FuzzySet("Negative", tfNegative),
            fsLittleNegative = new FuzzySet("LittleNegative", tfLittleNegative),
            fsZero = new FuzzySet("Zero", tfZero),
            fsLittlePositive = new FuzzySet("LittlePositive", tfLittlePositive),
            fsPositive = new FuzzySet("Positive", tfPositive),
            fsVeryPositive = new FuzzySet("VeryPositive", tfVeryPositive);
        #endregion

        #region SteeringWheel
        static FuzzySet fsLeftMax = new FuzzySet("LeftMax", tfLeftMax),
            fsLeftMedium = new FuzzySet("LeftMedium", tfLeftMedium),
            fsLeftMin = new FuzzySet("LeftMin", tfLeftMin),
            fsMiddle = new FuzzySet("Middle", tfMiddle),
            fsRightMin = new FuzzySet("RightMin", tfRightMin),
            fsRightMedium = new FuzzySet("RightMedium", tfRightMedium),
            fsRightMax = new FuzzySet("RightMax", tfRightMax);
        #endregion

        #region Friction
        private static FuzzySet fsNullFriction = new FuzzySet("Null", tfNullFriction),
            fsLowFriction = new FuzzySet("Low", tfLowFriction),
            fsMediumFriction = new FuzzySet("Medium", tfMediumFriction),
            fsHighFriction = new FuzzySet("High", tfHighFriction);
        #endregion

        #region Speed
        private static FuzzySet fsStopped = new FuzzySet("Stopped", tfStopped),
            fsVerySlow = new FuzzySet("VerySlow", tfVerySlow),
            fsSlow = new FuzzySet("Slow", tfSlow),
            fsRegular = new FuzzySet("Regular", tfRegular),
            fsLittleFast = new FuzzySet("LittleFast", tfLittleFast),
            fsFast = new FuzzySet("Fast", tfFast),
            fsVeryFast = new FuzzySet("VeryFast", tfVeryFast);
        #endregion

        #region Accelerator/Brake
        private static FuzzySet fsNone = new FuzzySet("None", tfNone),
            fsLittle = new FuzzySet("Little", tfLittle),
            fsMedium = new FuzzySet("Medium", tfMedium),
            fsMuch = new FuzzySet("Much", tfMuch);
        #endregion

        #endregion

        LinguisticVariable lvWheelAngle, lvSteeringWheel, lvFriction,
            lvSpeed, lvAccelerator, lvBrake;

        Database db;

        InferenceSystem IS;

        public FIS()
        {
            setupVariables();
            setupDatabase();
            setupInfSystem();
        }

        private void setupVariables()
        {
            #region WheelAngle
            lvWheelAngle = new LinguisticVariable("WheelAngle", -58.2f, 58.2f);
            lvWheelAngle.AddLabel(fsVeryNegative);
            lvWheelAngle.AddLabel(fsNegative);
            lvWheelAngle.AddLabel(fsLittleNegative);
            lvWheelAngle.AddLabel(fsZero);
            lvWheelAngle.AddLabel(fsLittlePositive);
            lvWheelAngle.AddLabel(fsPositive);
            lvWheelAngle.AddLabel(fsVeryPositive);
            #endregion

            #region SteeringWheel
            lvSteeringWheel = new LinguisticVariable("SteeringWheel", -2.5f, 2.5f);
            lvSteeringWheel.AddLabel(fsLeftMax);
            lvSteeringWheel.AddLabel(fsLeftMedium);
            lvSteeringWheel.AddLabel(fsLeftMin);
            lvSteeringWheel.AddLabel(fsMiddle);
            lvSteeringWheel.AddLabel(fsRightMin);
            lvSteeringWheel.AddLabel(fsRightMedium);
            lvSteeringWheel.AddLabel(fsRightMax);
            #endregion

            #region Friction
            lvFriction = new LinguisticVariable("Friction", 0, 10);
            lvFriction.AddLabel(fsNullFriction);
            lvFriction.AddLabel(fsLowFriction);
            lvFriction.AddLabel(fsMediumFriction);
            lvFriction.AddLabel(fsHighFriction);
            #endregion

            #region Speed
            lvSpeed = new LinguisticVariable("Speed", -5, 190);
            lvSpeed.AddLabel(fsStopped);
            lvSpeed.AddLabel(fsVerySlow);
            lvSpeed.AddLabel(fsSlow);
            lvSpeed.AddLabel(fsRegular);
            lvSpeed.AddLabel(fsLittleFast);
            lvSpeed.AddLabel(fsFast);
            lvSpeed.AddLabel(fsVeryFast);
            #endregion

            #region Accelerator
            lvAccelerator = new LinguisticVariable("Accelerator", 0, 1);
            lvAccelerator.AddLabel(fsNone);
            lvAccelerator.AddLabel(fsLittle);
            lvAccelerator.AddLabel(fsMedium);
            lvAccelerator.AddLabel(fsMuch);
            #endregion

            #region Brake
            lvBrake = new LinguisticVariable("Brake", 0, 1);
            lvBrake.AddLabel(fsNone);
            lvBrake.AddLabel(fsLittle);
            lvBrake.AddLabel(fsMedium);
            lvBrake.AddLabel(fsMuch);
            #endregion
        }

        private void setupDatabase()
        {
            db = new Database();
            db.AddVariable(lvWheelAngle);
            db.AddVariable(lvSteeringWheel);
            db.AddVariable(lvFriction);
            db.AddVariable(lvSpeed);
            db.AddVariable(lvAccelerator);
            db.AddVariable(lvBrake);
        }

        private void setupInfSystem()
        {
            IS = new InferenceSystem(db, new CentroidDefuzzifier(1000));

            #region WheelAngle
            IS.NewRule("TurnLeftMax", "IF SteeringWheel IS LeftMax THEN WheelAngle IS VeryNegative");
            IS.NewRule("TurnLeftMed", "IF SteeringWheel IS LeftMedium THEN WheelAngle IS Negative");
            IS.NewRule("TurnLeftMin", "IF SteeringWheel IS LeftMin THEN WheelAngle IS LittleNegative");
            IS.NewRule("GoStraight", "IF SteeringWheel IS Middle THEN WheelAngle IS Zero");
            IS.NewRule("TurnRightMin", "IF SteeringWheel IS RightMin THEN WheelAngle IS LittlePositive");
            IS.NewRule("TurnRightMed", "IF SteeringWheel IS RightMedium THEN WheelAngle IS Positive");
            IS.NewRule("TurnRightMax", "IF SteeringWheel IS RightMax THEN WheelAngle IS VeryPositive");
            #endregion

            IS.NewRule("Rule 1", "IF Speed IS Stopped AND Accelerator IS Much THEN Speed IS Slow");
            IS.NewRule("Rule 2", "IF Speed IS Stopped AND Accelerator IS Medium THEN Speed IS VerySlow");
            IS.NewRule("Rule 3","IF Speed IS Stopped AND Accelerator IS Little THEN Speed IS VerySlow");

            IS.NewRule("Rule 5", "IF Speed IS VerySlow AND Accelerator IS Much THEN Speed IS Regular");
            IS.NewRule("Rule 6", "IF Speed IS VerySlow AND Accelerator IS Medium THEN Speed IS Slow");
            IS.NewRule("Rule 7", "IF Speed IS VerySlow AND Accelerator IS Little THEN Speed IS VerySlow");

            IS.NewRule("Rule 9", "IF Speed IS Slow AND Accelerator IS Much THEN Speed IS LittleFast");
            IS.NewRule("Rule 10", "IF Speed IS Slow AND Accelerator IS Medium THEN Speed IS Regular");
            IS.NewRule("Rule 11", "IF Speed IS Slow AND Accelerator IS Little THEN Speed IS Slow");

            IS.NewRule("Rule 13", "IF Speed IS Regular AND Accelerator IS Much THEN Speed IS Fast");
            IS.NewRule("Rule 14", "IF Speed IS Regular AND Accelerator IS Medium THEN Speed IS LittleFast");
            IS.NewRule("Rule 15", "IF Speed IS Regular AND Accelerator IS Little THEN Speed IS Regular");

            IS.NewRule("Rule 17", "IF Speed IS LittleFast AND Accelerator IS Much THEN Speed IS VeryFast");
            IS.NewRule("Rule 18", "IF Speed IS LittleFast AND Accelerator IS Medium THEN Speed IS Fast");
            IS.NewRule("Rule 19", "IF Speed IS LittleFast AND Accelerator IS Little THEN Speed IS LittleFast");

            IS.NewRule("Rule 21", "IF Speed IS Fast AND Accelerator IS Much THEN Speed IS VeryFast");
            IS.NewRule("Rule 22", "IF Speed IS Fast AND Accelerator IS Medium THEN Speed IS VeryFast");
            IS.NewRule("Rule 23", "IF Speed IS Fast AND Accelerator IS Little THEN Speed IS Fast");

            IS.NewRule("Rule 25", "IF Speed IS VeryFast AND Brake IS Much THEN Speed IS LittleFast");
            IS.NewRule("Rule 26", "IF Speed IS VeryFast AND Brake IS Medium THEN Speed IS Fast");
            IS.NewRule("Rule 27", "IF Speed IS VeryFast AND Brake IS Little THEN Speed IS Fast");

            IS.NewRule("Rule 29", "IF Speed IS Fast AND Brake IS Much THEN Speed IS Slow");
            IS.NewRule("Rule 30", "IF Speed IS Fast AND Brake IS Medium THEN Speed IS Regular");
            IS.NewRule("Rule 31", "IF Speed IS Fast AND Brake IS Little THEN Speed IS LittleFast");

            IS.NewRule("Rule 33", "IF Speed IS LittleFast AND Brake IS Much THEN Speed IS VerySlow");
            IS.NewRule("Rule 34", "IF Speed IS LittleFast AND Brake IS Medium THEN Speed IS Slow");
            IS.NewRule("Rule 35", "IF Speed IS LittleFast AND Brake IS Little THEN Speed IS Regular");

            IS.NewRule("Rule 37", "IF Speed IS Regular AND Brake IS Much THEN Speed IS Stopped");
            IS.NewRule("Rule 38", "IF Speed IS Regular AND Brake IS Medium THEN Speed IS VerySlow");
            IS.NewRule("Rule 39", "IF Speed IS Regular AND Brake IS Little THEN Speed IS Slow");

            IS.NewRule("Rule 41", "IF Speed IS Slow AND Brake IS Much THEN Speed IS Stopped");
            IS.NewRule("Rule 42", "IF Speed IS Slow AND Brake IS Medium THEN Speed IS VerySlow");
            IS.NewRule("Rule 43", "IF Speed IS Slow AND Brake IS Little THEN Speed IS VerySlow");

            IS.NewRule("Rule 45", "IF Speed IS VerySlow AND Brake IS Much THEN Speed IS Stopped");
            IS.NewRule("Rule 46", "IF Speed IS VerySlow AND Brake IS Medium THEN Speed IS Stopped");
            IS.NewRule("Rule 47", "IF Speed IS VerySlow AND Brake IS Little THEN Speed IS Stopped");
        }

        public float getWheelAngle(float steeringWheel)
        {
            IS.SetInput("SteeringWheel", steeringWheel);
            return IS.Evaluate("WheelAngle");
        }

        public float getSpeed(float speed, float accelerator, float brake)
        {
            try
            {
                IS.SetInput("Speed", speed);
                IS.SetInput("Accelerator", accelerator);
                IS.SetInput("Brake", brake);
                return IS.Evaluate("Speed");
            }
            catch(Exception)
            {
                return speed;
            }
        }
    }
}
