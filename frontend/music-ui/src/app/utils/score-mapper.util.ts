/**
 * Score Mapper Utility
 * 
 * Converts user-facing labels into numeric scores for backend consumption.
 * Each label maps to a fixed range, and a random value within that range
 * is generated at answer time.
 * 
 * CRITICAL: Random values are generated ONCE per answer and must remain
 * constant for the entire session until restart.
 */

// Score range definitions
const SCORE_RANGES: Record<string, Record<string, [number, number]>> = {
    energy: {
        'Low': [0.10, 0.30],
        'Mid': [0.40, 0.60],
        'High': [0.70, 0.90]
    },
    emotion: {
        'Light': [0.30, 0.50],
        'Deep': [0.60, 0.80]
    },
    familiarity: {
        'Familiar': [0.60, 0.80],
        'Exploratory': [0.20, 0.40]
    },
    time: {
        'Past': [0.20, 0.40],
        'Timeless': [0.45, 0.55],
        'Now': [0.70, 0.90]
    }
};

/**
 * Generates a random score within the range defined for the given label.
 * 
 * @param parameterType - The parameter type ('energy', 'emotion', 'familiarity', 'time')
 * @param label - The user-selected label (e.g., 'Low', 'High', 'Light', etc.)
 * @returns A random number within the defined range, rounded to 2 decimal places
 * 
 * @example
 * generateScore('energy', 'High') // Returns a value between 0.70 and 0.90
 * generateScore('emotion', 'Light') // Returns a value between 0.30 and 0.50
 */
export function generateScore(parameterType: string, label: string): number {
    const ranges = SCORE_RANGES[parameterType];

    if (!ranges) {
        throw new Error(`Unknown parameter type: ${parameterType}`);
    }

    const range = ranges[label];

    if (!range) {
        throw new Error(`Unknown label "${label}" for parameter type "${parameterType}"`);
    }

    const [min, max] = range;

    // Generate random value within range
    const randomValue = min + Math.random() * (max - min);

    // Round to 2 decimal places
    return Math.round(randomValue * 100) / 100;
}
