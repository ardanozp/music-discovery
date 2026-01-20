import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import os
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor, GradientBoostingRegressor
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error, r2_score
from sklearn.preprocessing import StandardScaler

FILENAME = 'final_dataset.csv' 
target_col = 'Energy_kWh'

# 1. DATA LOADING AND CLEANING 
def load_and_clean_data(filename):
    try:
        current_dir = os.path.dirname(os.path.abspath(__file__))
    except NameError:
        current_dir = os.getcwd()

    possible_filenames = [filename, filename + '.csv']
    file_path = None
    for fname in possible_filenames:
        temp_path = os.path.join(current_dir, fname)
        if os.path.exists(temp_path):
            file_path = temp_path
            break
    
    if file_path is None:
        print(f"\n!!! ERROR: File '{filename}' not found.")
        return None

    print(f"Reading data: {file_path}")
    df = pd.read_csv(file_path)

    
    for col in df.columns:
        if col.lower() in ['timestamp', 'time', 'date']:
            df.rename(columns={col: 'Date'}, inplace=True)
            break
    

    if 'Date' in df.columns:
        df['Date'] = pd.to_datetime(df['Date'], errors='coerce')
        df.sort_values('Date', inplace=True)
    
    # Cleaning:
    initial_count = len(df)
    
    # Drop rows where target is NaN 
    df = df.dropna(subset=[target_col])
    # Drop Zero and Negative values 
    if pd.api.types.is_numeric_dtype(df[target_col]):
        df = df[df[target_col] > 0.1]
    # Outlier Cleaning 
    if len(df) > 0:
        mean_val = df[target_col].mean()
        std_val = df[target_col].std()
        cutoff = mean_val + (3 * std_val)

        if cutoff > 20000: cutoff = 20000
        
        df_clean = df[df[target_col] < cutoff].copy()
    else:
        df_clean = df.copy()
    
    print(f"Data Cleaning: {initial_count} -> {len(df_clean)} rows remaining.")
    if len(df_clean) > 0:
        print(f"Max Consumption Value (Cleaned): {df_clean[target_col].max()}")

    # Critical Missing Value Filling
    numeric_cols = df_clean.select_dtypes(include=[np.number]).columns
    df_clean[numeric_cols] = df_clean[numeric_cols].interpolate(method='linear', limit_direction='both')
    df_clean[numeric_cols] = df_clean[numeric_cols].fillna(method='bfill').fillna(method='ffill')
    df_clean[numeric_cols] = df_clean[numeric_cols].fillna(0)
    
    return df_clean

#  2. FEATURE ENGINEERING 
def feature_engineering(df):
    print("Generating features...")
    if 'Date' in df.columns:
        df = df.dropna(subset=['Date'])
        df['hour'] = df['Date'].dt.hour
        df['day_of_week'] = df['Date'].dt.dayofweek
        df['month'] = df['Date'].dt.month
        df['is_weekend'] = df['day_of_week'].apply(lambda x: 1 if x >= 5 else 0)
        df['hour_sin'] = np.sin(2 * np.pi * df['hour'] / 24)
        df['hour_cos'] = np.cos(2 * np.pi * df['hour'] / 24)
    return df

#  3. VISUAL ANALYSIS (CORRELATION) 
def plot_correlation(df):
    plt.figure(figsize=(12, 10))
    numeric_df = df.select_dtypes(include=[np.number])
    if numeric_df.empty:
        print("Not enough numeric data for correlation plot.")
        return
        
    corr = numeric_df.corr()
    sns.heatmap(corr, annot=False, cmap='coolwarm', fmt=".2f")
    
    # HOCANIN İSTEĞİ: Başlık kaldırıldı.
    # plt.title("Feature Correlation Matrix") 
    
    plt.show()

# 4. MODEL TRAINING AND COMPARISON 
def train_compare_models(df):
    print("\nComparing models...")
    
    potential_features = ['airTemperature', 'cloudCoverage', 'dewTemperature', 
                          'precipDepth1HR', 'seaLvlPressure', 'windDirection', 
                          'windSpeed', 'hour', 'day_of_week', 'month', 'is_weekend',
                          'hour_sin', 'hour_cos']
    
    
    features = [col for col in potential_features if col in df.columns]
    
    #  Final Check: NaN and INF Cleaning 
    X = df[features].copy()
    y = df[target_col].copy()
    
    # Replace infinite values with NaN and drop
    X.replace([np.inf, -np.inf], np.nan, inplace=True)
    X.dropna(inplace=True)
    # Align Y with X 
    y = y.loc[X.index]
    # If NaNs persist, fill with 0
    X.fillna(0, inplace=True)
    
    print(f"Rows used for training: {len(X)}")
    
    if len(X) < 10:
        print("!!! ERROR: Not enough data left for training. Cleaning criteria might be too strict.")
        return {}, "None", [], [], []

    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42, shuffle=False)
    
    scaler = StandardScaler()
    try:
        X_train_scaled = scaler.fit_transform(X_train)
        X_test_scaled = scaler.transform(X_test)
    except ValueError as e:
        print(f"StandardScaler Error: {e}")
        print("Dataset may contain non-numeric or corrupted values.")
        return {}, "None", [], [], []
    
    # List of Models
    models = {
        "Linear Regression": LinearRegression(),
        "Random Forest": RandomForestRegressor(n_estimators=100, random_state=42, n_jobs=-1),
        "Gradient Boosting": GradientBoostingRegressor(n_estimators=100, random_state=42)
    }
    
    results = {}
    best_model = None
    best_score = -np.inf
    best_name = ""
    
    for name, model in models.items():
        print(f"Training: {name}...")
        try:
            model.fit(X_train_scaled, y_train)
            y_pred = model.predict(X_test_scaled)
            
            rmse = np.sqrt(mean_squared_error(y_test, y_pred))
            r2 = r2_score(y_test, y_pred)
            
            results[name] = {'RMSE': rmse, 'R2': r2, 'Prediction': y_pred, 'Model': model}
            print(f"  -> RMSE: {rmse:.2f}, R2: {r2:.4f}")
            
            if r2 > best_score:
                best_score = r2
                best_model = model
                best_name = name
        except Exception as e:
            print(f"  !!! Error occurred while training {name}: {e}")

    if best_name == "":
        print("No models trained successfully.")
        return results, "None", features, X_test_scaled, y_test

    print(f"\n--- BEST MODEL: {best_name} (R2: {best_score:.4f}) ---")
    return results, best_name, features, X_test_scaled, y_test

#  5. RESULT VISUALIZATION 
def plot_results_and_importance(results, best_name, feature_names, X_test_scaled, y_test):
    if not results or best_name == "None":
        print("No results, cannot plot.")
        return

    # Graph 1: Prediction vs Actual (For the best model)
    limit = 200
    
    plt.figure(figsize=(15, 6))
    plt.plot(range(len(y_test[:limit])), y_test.values[:limit], label='Actual Consumption', color='black', alpha=0.7)
    
    # Plot all models (For comparison)
    colors = {'Linear Regression': 'red', 'Random Forest': 'orange', 'Gradient Boosting': 'green'}
    for name, data in results.items():
        if 'Prediction' in data:
            if name == best_name:
                lw = 2 
            else:
                lw = 1
            plt.plot(range(len(data['Prediction'][:limit])), data['Prediction'][:limit], label=f'{name} Prediction', color=colors.get(name, 'blue'), linestyle='--', linewidth=lw)
        
    # HOCANIN İSTEĞİ: Başlık kaldırıldı.
    # plt.title(f'Model Comparison (Winner: {best_name})') 
    
    plt.xlabel('Time Step') 
    plt.ylabel('Energy (kWh)') 
    plt.legend()
    plt.grid(True, alpha=0.3)
    
    # HOCANIN İSTEĞİ: Kutu (frame) kaldırıldı, sadece X ve Y eksen çizgileri kalsın.
    ax = plt.gca()
    ax.spines['top'].set_visible(False)
    ax.spines['right'].set_visible(False)
    
    plt.show()
    
    # Graph 2: Feature Importance (Tree-based models only)
    best_model = results[best_name]['Model']
    if hasattr(best_model, 'feature_importances_'):
        importances = best_model.feature_importances_
        indices = np.argsort(importances)[::-1]
        
        plt.figure(figsize=(10, 6))
        
        # HOCANIN İSTEĞİ: Başlık kaldırıldı.
        # plt.title(f"{best_name} - Feature Importance Levels") 
        
        plt.bar(range(len(importances)), importances[indices], align="center", color='skyblue')
        plt.xticks(range(len(importances)), [feature_names[i] for i in indices], rotation=45)
        
        # HOCANIN İSTEĞİ: Kutu (frame) kaldırıldı.
        ax = plt.gca()
        ax.spines['top'].set_visible(False)
        ax.spines['right'].set_visible(False)

        plt.tight_layout()
        plt.show()

# xxxxxxxxxxxxxx
#  Main Flow
# xxxxxxxxxxxxxx

if __name__ == "__main__":
    # HOURLY ANALYSIS
    print("\n" + "="*40)
    print("   SAATLİK (HOURLY) ANALİZ BAŞLIYOR")
    print("="*40)
    
    df = load_and_clean_data(FILENAME)
    
    if df is not None:
        df = feature_engineering(df)
        
        plot_correlation(df) 
        results, best_name, feature_names, X_test_scaled, y_test = train_compare_models(df)
        plot_results_and_importance(results, best_name, feature_names, X_test_scaled, y_test)


        # DAILY ANALYSIS
        print("\n\n" + "="*40)
        print("   GÜNLÜK (DAILY) ANALİZ BAŞLIYOR")
        print("="*40)
        
        
        if 'Date' in df.columns:
            
            numeric_cols = df.select_dtypes(include=[np.number]).columns
            
            
            df_daily = df.resample('D', on='Date')[numeric_cols].mean()
            
            df_daily = df_daily.dropna()
            
            df_daily = df_daily.reset_index()
            
            print(f"Günlük Veri Seti Boyutu: {len(df_daily)}")
            
            
            df_daily = feature_engineering(df_daily)
            
            
            results_day, best_name_day, feats_day, X_test_day, y_test_day = train_compare_models(df_daily)
            
            
            plot_results_and_importance(results_day, best_name_day, feats_day, X_test_day, y_test_day)
        else:
            print("ERROR")
